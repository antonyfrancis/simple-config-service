using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SimpleConfigService.AspNet5
{
    public class CustomJsonConfigurationFileParser
    {
		private readonly IDictionary<string, string> _data =
			(IDictionary<string, string>)
				new SortedDictionary<string, string>((IComparer<string>)StringComparer.OrdinalIgnoreCase);

		private readonly Stack<string> _context = new Stack<string>();
		private string _currentPath;
		private JsonTextReader _reader;

		public IDictionary<string, string> Parse(Stream input)
		{
			this._data.Clear();
			this._reader = new JsonTextReader((TextReader)new StreamReader(input));
			this._reader.DateParseHandling = DateParseHandling.None;
			this.VisitJObject(JObject.Load((JsonReader)this._reader));
			return this._data;
		}

		private void VisitJObject(JObject jObject)
		{
			foreach (JProperty property in jObject.Properties())
			{
				this.EnterContext(property.Name);
				this.VisitProperty(property);
				this.ExitContext();
			}
		}

		private void VisitProperty(JProperty property)
		{
			this.VisitToken(property.Value);
		}

		private void VisitToken(JToken token)
		{
			switch (token.Type)
			{
				case JTokenType.Object:
					this.VisitJObject(Extensions.Value<JObject>((IEnumerable<JToken>)token));
					break;
				case JTokenType.Array:
					this.VisitArray(Extensions.Value<JArray>((IEnumerable<JToken>)token));
					break;
				case JTokenType.Integer:
				case JTokenType.Float:
				case JTokenType.String:
				case JTokenType.Boolean:
				case JTokenType.Null:
				case JTokenType.Raw:
				case JTokenType.Bytes:
					this.VisitPrimitive(token);
					break;
				default:
					throw new FormatException("Unsupported format $((object) this._reader.TokenType) $((object) this._reader.Path) $((object) this._reader.LineNumber) $((object) this._reader.LinePosition))");
			}
		}

		private void VisitArray(JArray array)
		{
			for (int index = 0; index < array.Count; ++index)
			{
				this.EnterContext(index.ToString());
				this.VisitToken(array[index]);
				this.ExitContext();
			}
		}

		private void VisitPrimitive(JToken data)
		{
			string key = this._currentPath;
			if (this._data.ContainsKey(key))
				throw new FormatException("Key is duplicated $((object) key))");
			this._data[key] = data.ToString();
		}

		private void EnterContext(string context)
		{
			this._context.Push(context);
			this._currentPath = string.Join(":", Enumerable.Reverse<string>((IEnumerable<string>)this._context));
		}

		private void ExitContext()
		{
			this._context.Pop();
			this._currentPath = string.Join(":", Enumerable.Reverse<string>((IEnumerable<string>)this._context));
		}
	}
}
