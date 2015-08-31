console.log('Loading function');

var doc = require('dynamodb-doc');
var dynamo = new doc.DynamoDB();

exports.handler = function (event, context) {
	//console.log('Received event:', JSON.stringify(event, null, 2));
	
	var operation = event.operation;
	delete event.operation;
	
	event.Key.Application = event.ApiKey + "_" + event.Key.Application;
	delete event.ApiKey;
	
	switch (operation) {
		case 'create':
			dynamo.putItem(event, context.done);
			break;
		case 'read':
			dynamo.getItem(event, context.done);
			break;
		case 'update':
			dynamo.updateItem(event, context.done);
			break;
		case 'delete':
			dynamo.deleteItem(event, context.done);
			break;
		case 'list':
			dynamo.scan(event, context.done);
			break;
		case 'echo':
			context.succeed(event);
			break;
		case 'ping':
			context.succeed('pong');
			break;
		default:
			context.fail(new Error('Unrecognized operation "' + operation + '"'));
	}
};