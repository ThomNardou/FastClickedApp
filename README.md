# FastClickedApp
An simple asp.net application dockerized 

## Instalation 

Pour commencer à utiliser le programme il est necessaire d'installer le repos github sur votre poste. </br>

Une fois l'installation terminé il est necessaire de se diriger dans le dossier "docker-compose" et d'ouvrir une fenêtre CMD :

<img src="./image/cmd.png">

Une fois sur la page il est necessaire d'executer cette commande : 
```bash
docker compose up
```
Une fois que l'execution s'est terminée vous remaquerez qu'une conteneur s'est créer.

</br>
L'application peut maintenant être lancée en ouvrant le projet depuis Visual Studio. Après avoir démarré l'application, un message d'erreur spécifique doit apparaître que voici : 
<img src="./image/error.png">

Pour pouvoir résoudre le problème il est necessaire d'executer cette commande toujours dans un environnement CMD

```bash
docker network connect --ip 192.168.0.2 docker-compose_db_network FastClieckWebAppMVC
```
