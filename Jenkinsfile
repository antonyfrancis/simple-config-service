pipeline {
  agent any
  stages {
    stage('Build') {
      steps {
        echo 'Compiling'
      }
    }
    stage('Deploy to Stage') {
      steps {
        echo 'Deploying to stage'
      }
    }
    stage('Deploy to Prod') {
      steps {
        input 'Are you sure?'
      }
    }
    stage('Complete') {
      steps {
        echo 'Deploy complete'
      }
    }
  }
}