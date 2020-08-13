
##### Create SQS Queue
```
awslocal sqs create-queue --queue-name qc-request
```

Arn for queue:
> arn:aws:sqs:us-east-1:000000000000:qc-request

##### Create Role
```
awslocal iam create-role --role-name lambda-role --assume-role-policy-document file://testrole.json
```

##### Build Lambda
```
dotnet tool run dotnet-lambda package -pl ./src/TestLambdas/ -o ./output/TestLambda.zip -c Release -f netcoreapp3.1 --msbuild-parameters "--self-contained true"
```

##### Create Lambda Function
```
awslocal lambda create-function --function-name test-lambda-event --runtime dotnetcore3.1 --memory-size 256 --timeout 60 --zip-file fileb://output/TestLambda.zip --handler TestLambdas::TestLambdas.TriggerFromEventBridge::HandlerEvent --role arn:aws:iam::000000000000:role/lambda-role

awslocal lambda create-function --function-name test-lambda-object --runtime dotnetcore3.1 --memory-size 256 --timeout 60 --zip-file fileb://output/TestLambda.zip --handler TestLambdas::TestLambdas.TriggerFromEventBridge::HandlerObject --role arn:aws:iam::000000000000:role/lambda-role
```

##### Create Event Bus
```
awslocal events create-event-bus --name test-event-bus

awslocal events put-rule --name test-event-rule --event-bus-name test-event-bus --event-pattern "{\"source\":[\"test.events\"],\"detail-type\":[\"TriggerEventLambda\"]}"
awslocal events put-targets --rule test-event-rule --event-bus-name test-event-bus --targets "Id"="1","Arn"="arn:aws:lambda:us-east-1:000000000000:function:test-lambda-event"

awslocal events put-rule --name test-object-rule --event-bus-name test-event-bus --event-pattern "{\"source\":[\"test.events\"],\"detail-type\":[\"TriggerObjectLambda\"]}"
awslocal events put-targets --rule test-object-rule --event-bus-name test-event-bus --targets "Id"="1","Arn"="arn:aws:lambda:us-east-1:000000000000:function:test-lambda-object"
```

##### Set permissions on Lambda for the EventBus
```
awslocal lambda add-permission --function-name test-lambda-event --statement-id test-event-rule-statement --action 'lambda:InvokeFunction' --principal events.amazonaws.com --source-arn arn:aws:events:us-east-1:000000000000:rule/test-event-rule

awslocal lambda add-permission --function-name test-lambda-object --statement-id test-object-rule-statement --action 'lambda:InvokeFunction' --principal events.amazonaws.com --source-arn arn:aws:events:us-east-1:000000000000:rule/test-object-rule
```

##### Put Event on the Event Bus
```
awslocal events put-events --entries file://testevent.json
```

##### If the code is updated, need to update lambda
```
awslocal lambda update-function-code --function-name test-lambda-event --zip-file fileb://output/TestLambda.zip

awslocal lambda update-function-code --function-name test-lambda-object --zip-file fileb://output/TestLambda.zip
```