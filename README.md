# Express Voitures

## À propos du projet

### Description

**Express Voitures** est une application web de gestion de véhicules destinée aux concessionnaires automobiles et aux gestionnaires de flottes. 
Elle permet de suivre et de gérer divers aspects liés aux véhicules, notamment leur état, leurs réparations, et les détails d'achat et de vente. 
L'application est construite avec le framework ASP.NET Core et utilise SQL Server pour la gestion des données.

### Fonctionnalités Principales

- **Gestion des Véhicules** : Permet d'ajouter, de modifier et de supprimer des véhicules.
  Chaque véhicule peut être décrit par des détails tels que le VIN (numéro d'identification du véhicule), l'année, la marque, le modèle, la finition, le prix d'achat, et la date d'achat.

- **Suivi des Réparations** : Enregistrez les réparations effectuées sur les véhicules. Chaque réparation comprend une description et un coût associé.

- **Suivi de l'État des Véhicules** : Chaque véhicule peut être marqué comme "Disponible" ou "Vendu". Cela permet de suivre facilement l'état de chaque véhicule.

- **Gestion des Photos des Véhicules** : Téléchargez et affichez des photos pour chaque véhicule, permettant ainsi une meilleure visualisation.

- **Sécurité et Autorisations** : Gestion des accès et des autorisations pour différentes actions, comme la création, la modification et la suppression de véhicules.

### Ce Qui a Été Fait

- **Mise en Place de la Structure du Projet** : Création de la solution ASP.NET Core avec une architecture MVC (Modèle-Vue-Contrôleur).

- **Modèle de Données** : Définition des modèles de données pour représenter les véhicules (`Car`), les marques (`Make`), les modèles de voitures (`Model`), et les réparations (`Repair`).

- **Services et Répertoires** : Mise en place des services pour encapsuler la logique métier et des répertoires pour l'accès aux données. Cela inclut l'utilisation d'interfaces pour respecter les principes SOLID.

- **Contrôleurs** : Développement des contrôleurs pour gérer les requêtes HTTP et coordonner les interactions entre les vues et les modèles.

- **Vues** : Création des vues Razor pour l'interface utilisateur, permettant d'afficher, de créer, de modifier et de supprimer des véhicules.

- **JavaScript pour la Dynamique de l'Interface** : Ajout de scripts JavaScript pour gérer des fonctionnalités dynamiques comme l'affichage conditionnel de la date de vente selon le statut du véhicule.

- **Gestion des Statuts des Véhicules** : Mise en place de la logique pour suivre et mettre à jour le statut des véhicules, y compris la gestion de la disponibilité et la date de vente.

- **Déploiement sur Azure** : Configuration et déploiement de l'application sur Azure, incluant la mise en place d'une base de données SQL Azure.

### Technologies Utilisées

- **ASP.NET Core** : Pour la structure MVC et le développement back-end.
- **Entity Framework Core** : Pour l'accès et la gestion de la base de données.
- **SQL Server** : Pour le stockage des données.
- **Razor** : Pour la création de vues dynamiques.
- **JavaScript** : Pour les interactions dynamiques sur l'interface utilisateur.
- **Azure** : Pour l'hébergement de l'application web et de la base de données.

---

# Express Voitures

Express Voitures est une application de gestion de véhicules permettant de suivre l'état, les réparations et les détails d'achat/vente des voitures. 
Cette application est construite avec ASP.NET Core et utilise une base de données SQL Server.

## Table des Matières

- [Prérequis](#prérequis)
- [Installation](#installation)
- [Configuration](#configuration)
- [Utilisation](#utilisation)
- [Déploiement sur Azure](#déploiement-sur-azure)

## Prérequis

Avant de commencer, assurez-vous d'avoir les éléments suivants installés sur votre machine :

- [Visual Studio 2022](https://visualstudio.microsoft.com/vs/)
- [SQL Server](https://www.microsoft.com/fr-fr/sql-server/sql-server-downloads)
- [Azure Account](https://azure.microsoft.com/en-us/free/)

## Installation

1. Clonez le dépôt :

   ```bash
   git clone https://github.com/ElodieCara/P5-Express-Voitures.git
   cd express-voitures
   ```

2. Ouvrez le projet dans Visual Studio.

3. Restaurer les packages NuGet :

   ```bash
   dotnet restore
   ```

4. Mettez à jour la chaîne de connexion dans `appsettings.json` avec vos informations SQL Server :

   ```json  
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=ExpressVoitures;Trusted_Connection=True;MultipleActiveResultSets=true;"
  }
}

   ```

## Configuration

1. Créez la base de données :

   ```bash
   dotnet ef database update
   ```

## Utilisation

1. Lancez l'application depuis Visual Studio ou avec la commande :

   ```bash
   dotnet run
   ```

2. Accédez à l'application via votre navigateur à l'adresse `https://localhost:5001`.

## Déploiement sur Azure

1. Créez un compte Azure si vous n'en avez pas déjà un.

2. Suivez [ce guide](https://learn.microsoft.com/fr-fr/aspnet/core/tutorials/publish-to-azure-webapp-using-vs) pour déployer l'application sur Azure App Service.


