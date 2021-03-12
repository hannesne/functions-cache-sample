$serviceName = "functions-cache-signalr"
$resourcegroup = "functions-cache-sample"

# Create resource group 
az group create --name $resourcegroup --location eastus

# Create the Azure SignalR Service resource
az signalr create --name $serviceName --resource-group $resourcegroup --sku Standard_S1 --unit-count 1 --service-mode Serverless

# Get the SignalR primary connection string 
$primaryConnectionString = $(az signalr key list --name $serviceName --resource-group $resourcegroup --query primaryConnectionString -o tsv)

echo "$primaryConnectionString"