### About the project
This was the BSI team's project in the IATA ONE Record Hackathon.Written using .NET 8 technology, this is an API back-end service.  

backend: https://github.com/bsi-iata/backend  
frontend: https://github.com/bsi-iata/frontend   

### How to deploy projects

Create the configs directory and add a configuration file in json format to the directory. The name of the file is optional.

For example, the config.json file contains the following contents:

```json
{
  "UseAzureOpenAI": true,
  "AzureOpenAI": {
    "Endpoint": "https://xxx.openai.azure.com",
    "ApiKey": "xxx",
    "VersionPreviewDeploymentName": "xxx",
    "ChatCompletionDeploymentName": "xx",
    "VersionPreviewMaxTokens": 2000,
    "ChatCompletionMaxTokens": 1000
  },
  "OpenAI": {
    "OpenAIApiKey": null,
    "VersionPreviewDeploymentName": null,
    "ChatCompletionDeploymentName": null,
    "VersionPreviewMaxTokens": 2000,
    "ChatCompletionMaxTokens": 1000,
    "OpenAIOrgId": null
  },
  "ConnectionStrings": {
    "MySql": "Server=0.0.0.0;Port=3306;Database=iata;Uid=root;Pwd=123456;"
  },
  "Server": "https://aaaaxxx.com/"
}
```

Configuration description.

* `UseAzureOpenAI` : If you are using Azure Open AI, turn this option on with a value of true or false.
* `VersionPreviewDeploymentName`: An AI model with visual recognition,such as gpt-4-version-preview.
* `ChatCompletionDeploymentName`: ChatCompletion AI model,such as gtp-4-32k.
* `VersionPreviewMaxTokens` , `ChatCompletionMaxTokens`: max tokens.
* `Server`: The service address of the program, when image detection, the picture needs to be stored in a static directory, and the url address of the picture is provided for the AI.


docker build and run:
```bash
docker build -t bsi-iata:1.0 .
docker run -itd -p 8080:8080 -p 8081:8081 -v /root/bsi/configs:/app/configs bsi-iata:1.0
```

You can deploy mysql services in this way:

```bash
 docker run --name mysql -p 3306:3306 -v /data/mysql:/var/lib/mysql -e MYSQL_ROOT_PASSWORD=123456 -itd mysql:8.0
```