# 23207-tp2

Prototype statique des écrans du système de simulation des barrières de péage. Application de bureau .NET 8 (Avalonia).

## Prérequis

- SDK .NET 8
- Un environnement graphique (Windows, macOS ou Linux avec session de bureau)

Vérifier le SDK :

```bash
dotnet --version
```

La version affichée doit commencer par `8.`.

## Build

Depuis la racine du dépôt :

```bash
dotnet restore Stationnement.sln
dotnet build Stationnement.sln
```

La compilation produit `src/Stationnement.Prototype/bin/Debug/net8.0/Stationnement.Prototype.dll`.

## Run

Depuis la racine du dépôt :

```bash
dotnet run --project src/Stationnement.Prototype/Stationnement.Prototype.csproj
```

La fenêtre « Simulation des barrières de péage » s’ouvre. L’accueil mène à la borne d’entrée, à la borne de sortie et au poste du gestionnaire.

## Visual Studio

1. Ouvrir `Stationnement.sln`.
2. Restaurer les paquets NuGet si Visual Studio le propose.
3. Définir `Stationnement.Prototype` comme projet de démarrage.
4. Lancer avec F5.

Au premier chargement, la restauration télécharge Avalonia 11.2.5.
