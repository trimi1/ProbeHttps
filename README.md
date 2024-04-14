# Probe HTTPS

## Prérequis

- C# version utilisée : .NET 6

## Compilation
##Librairies utilisées
NewtTonSoft.JSON Version 13.0.3
BouncyCastle.NetCore Version 2.2.1
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
