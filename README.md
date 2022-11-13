
kubectl apply -f https://raw.githubusercontent.com/kubernetes/ingress-nginx/controller-v1.5.1/deploy/static/provider/cloud/deploy.yaml

kubectl get pods --namespace=ingress-nginx

kubectl apply -f local-pvc.yaml

kubectl create secret generic mssql --from-literal=SA_PASSWORD="pa55w0rd!"

dotnet tool install --global dotnet-ef

註解 platformservice startup devloyment 相關設定

dotnet ef migrations add initialmigration

dotnet ef database update -- --environment Production

kubectl apply -f mssql-plat-depl.yaml

kubectl apply -f rabbitmq-depl.yaml

kubectl apply -f platforms-depl.yaml

kubectl apply -f commands-depl.yaml

docker build -t username/platformservice .

docker push username/platformservice

docker build -t username/commandsservice .

docker push username/commandsservice

kubectl get deployments

kubectl get services

kubectl get pods

kubectl get namespace

kubectl get pvc

kubectl delete deployment yamlName
#重啟
kubectl rollout restart deployment yamlName