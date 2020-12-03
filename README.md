# Solar Stack
Retrieves and forwards Fronius inverter data from the local subnet and pushes it to AWS

Data is forwarded to SQS and converted by a Lambda into a aggregated time series stored in DynamoDB

### Future Plans
* Merge local weather data into the time series
* Use AWS Forecaster to predict the next 24 hours of solar ouput
