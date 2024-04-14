# Probe HTTPS
La probe https interroge les services http suivants:
### Services 
- http1!https://www.swilabus.com/!0!1500        Réponse HTTP: 200 
- http2!https://www.swilabus.be/!0!2000         Réponse HTTP: 307
- http3!https://www.swilabus.com/trkr1!0!1700   Réponse HTTP: 402
- http4!https://www.swilabus.com/trkr2!0!1800   Réponse HTTP: 505
## Prérequis
- C# version utilisée : .NET 6
- Cloner le projet depuis le dépôt distant : https://github.com/SxYxuse/ProbeHttps.git
- La grammaire devrait déjà être compilée et ajoutée au projet
### Librairies utilisées
- NewtTonSoft.JSON Version 13.0.3
- BouncyCastle.NetCore Version 2.2.1
### Fichier de config
Le fichier de configuration JSON doit avoir la structure suivante :
```json
{
  "protocol": "https", // Le protocole à utiliser pour les requêtes HTTPS.
  "multicastAddress": "224.0.0.254", // L'adresse IP pour le multicast.
  "multicastPort": 60202, // Le port pour le multicast.
  "multicastInterface": "en9", // Votre interface réseau à utiliser pour le multicast.
  "multicastDelay": 90000, // Le délai entre chaque annonce multicast en millisecondes.
  "unicastPort": 60000, // Le port pour l'unicast.
  "aesKey": "EPCtxxGqG1HUvD1LWaAbjLp+iU57QoKtcwn/yX8yM0g=" // La clé AES pour le chiffrement.
}
```
### Exécution 
- /!\ Attention bien faire attention à se trouver dans la solution, et non dans le dossier du projet (cd .\ProbeHttps\)
- Utiliser la commande suivante afin d'exécuter le projet :

```bash
dotnet run -- Resource/config.json
