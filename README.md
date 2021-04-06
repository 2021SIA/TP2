# Genetic Algorithms Engine

## Build
Requires .Net 3.1 SDK

```console
dotnet build -c Release
```

The project can also be imported in Visual Studio 2019

## Run

```console
cd TP2\bin\Release\netcoreapp3.1\
.\TP2.exe [options]
```
Information about valid arguments can be found with the --help command

```console
.\TP2.exe --help

TP2:
  Genetic Algorithms Engine

Usage:
  TP2 [options]

Options:
  --config <config>      Configuration yaml file path
  --dataset <dataset>    Path to dataset directory
  --listen               Flag to run engine as web server
  --version              Show version information
  -?, -h, --help         Show help and usage information
```
The engine can be run with a local configuration .yaml file or can be set to listen on port 5000 with the option --listen to be used with a graphical interface in a React application. 

## Run with Configuration

```console
.\TP2.exe --dataset Dataset\allitems --config config.yaml
```

The configuration file must have the following format:

```console
n: <population size>
k: <parent selection size>
method1: <elite|roulette|ranking|boltzmann|tournament det|tournament prob|universal>
method2: <elite|roulette|ranking|boltzmann|tournament det|tournament prob|universal>
method3: <elite|roulette|ranking|boltzmann|tournament det|tournament prob|universal>
method4: <elite|roulette|ranking|boltzmann|tournament det|tournament prob|universal>
a: <A value>
b: <B value>
crossoverMethod: <one point|two point|anular|uniform>
mutationMethod: <gene|multigene limited|multigene uniform|complete>
mutationProbability: <mutation probability>
replacementMethod: <fill all|fill parent>
characterType: <warrior|archer|defender|rogue>
finish: <time|generations|acceptable solution|structure|content>
generationsLimit: <generation limit> (only with finish='generations','structure','content')
tournamentM: <tournament M value> (only with selection method='tournament det','torunament prob')
timeLimit: <time limit> (only with finish='time')
targetFitness: <target fitness> (only with finish='acceptable solution')
structurePercentage: <equal structure percentage> (only with finish='structure')
fitnessUnchanged: <fitness unchanged variation> (only with finish='content')
heightMutationDelta: <delta value to mutate height>
boltzmannK: <boltzmann k value> (only with selection method='boltzmann')
boltzmannT0: <boltzmann T0 value> (only with selection method='boltzmann')
boltzmannTc: <boltzmann Tc value> (only with selection method='boltzmann')
```

## Run with graphic interface (with realtime charts)

```console
.\TP2.exe --dataset Dataset\allitems --listen
```

Once the engine has run and is listening, go to TP2/Graphics where there is located a small React application to use as a graphic interface and run:

```console
npm install
npm start
```
Note: Node version must be v12

Once the application installs and starts, it can be used in a web browser at http://localhost:3000.



