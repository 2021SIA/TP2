import React from 'react';
import ReactApexChart from 'react-apexcharts';
import ApexCharts from 'apexcharts';
import './App.css';

const CharacterItem = (props) => {
    return (
        <div style={{ marginTop: 16 + 'px' }}>
            <div className="item-stat">
                <b>ID: </b>{props.item.id} <br />
            </div>
            <div className="item-stat">
                <b>Fuerza: </b>{props.item.force.toFixed(2)} <br />
            </div>
            <div className="item-stat">
                <b>Agilidad: </b>{props.item.agility.toFixed(2)} <br />
            </div>
            <div className="item-stat">
                <b>Pericia: </b>{props.item.expertise.toFixed(2)} <br />
            </div>
            <div className="item-stat">
                <b>Resistencia: </b>{props.item.resistance.toFixed(2)} <br />
            </div>
            <div className="item-stat">
                <b>Vida: </b>{props.item.health.toFixed(2)} <br />
            </div>
        </div>
    );
} 
class App extends React.Component {
  
  
  constructor(props) {
    super(props);
    this.storedAvgFitness = [];
    this.storedMinFitness = [];
    this.storedMaxFitness = [];
	this.storedDiversity = [];
    this.state = {
      config: {
        n: 1000,
        k: 500,
        method1: 'elite',
        method2: 'ranking',
        method3: 'roulette',
        method4: 'tournament det',
        a: 0.7,
        b: 0.3,
        crossoverMethod: 'uniform',
        mutationMethod: 'gene',
        mutationProbability: 0.1,
        replacementMethod: 'fill all',
        characterType: 'warrior',
        finish: 'generations',
        generationsLimit: 1000,
        tournamentM: 3,
        timeLimit: "",
        targetFitness: "",
        structurePercentage: "",
		fitnessUnchanged: "",
		heightMutationDelta: 0.05
      },
      fitnessSeries: [{
        name:"Fitness Promedio",
        data: this.storedAvgFitness
      },{
        name:"Fitness Maximo",
        data: this.storedMaxFitness
      },{
        name:"Fitness Minimo",
        data: this.storedMinFitness
      }],
	  diversitySeries: [{
		name: "Diversidad",
		data: this.storedDiversity
	  }],
      fitnessChartOptions: {
        chart: {
          id: 'fitness',
          height: "100%",
          type: 'line',
          animations: {
            enabled: true,
            easing: 'linear',
            dynamicAnimation: {
              speed: 200
            }
          },
          toolbar: {
            show: false
          },
          zoom: {
            enabled: false
          }
        },
        stroke: {
          curve: 'smooth'
        },
        title: {
          text: 'Fitness en tiempo real',
          align: 'center',
          style: {
            fontSize:  '30px',
            fontWeight:  'bold',
            color: '#263238'
          }
        },
        markers: {
          size: 0
        },
        xaxis: {
          type: 'numeric',
          max:500,
          range:500,
        },
        yaxis: {
          max: 35,
          min: 10,
          labels: {
            formatter: function (value) {
              return value.toFixed(2);
            }
          }
        },
        legend: {
          show: true
        },
      },
      diversityChartOptions: {
        chart: {
          id: 'diversity',
          height: "100%",
          type: 'line',
          animations: {
            enabled: true,
            easing: 'linear',
            dynamicAnimation: {
              speed: 200
            }
          },
          toolbar: {
            show: false
          },
          zoom: {
            enabled: false
          }
        },
        stroke: {
          curve: 'smooth'
        },
        title: {
          text: 'Diversidad genetica en tiempo real',
          align: 'center',
          style: {
            fontSize:  '30px',
            fontWeight:  'bold',
            color: '#263238'
          }
        },
        markers: {
          size: 0
        },
        xaxis: {
          type: 'numeric',
          max:500,
          range:500,
        },
        yaxis: {
          max: 5,
          min: 0,
          labels: {
            formatter: function (value) {
              return value.toFixed(2);
            }
          }
        },
        legend: {
          show: true
        },
      },
      running: false,
      bestCharacter: null
    
    };
  }
  handleNChange(event) {
    const config = this.state.config;
    config.n = event.target.value;
    this.setState({ config });
  }
  handleKChange(event) {
    const config = this.state.config;
    config.k = event.target.value;
    this.setState({ config });
  }
  handleAChange(event) {
    const config = this.state.config;
    config.a = event.target.value;
    this.setState({ config });
  }
  handleBChange(event) {
    const config = this.state.config;
    config.b = event.target.value;
    this.setState({ config });
  }
  handleCrossoverChange(event) {
    const config = this.state.config;
    config.crossoverMethod = event.target.value;
    this.setState({ config });
  }
  handleSelectionAChange(event) {
    const config = this.state.config;
    config.method1 = event.target.value;
    this.setState({ config });
  }
  handleSelectionBChange(event) {
    const config = this.state.config;
    config.method2 = event.target.value;
    this.setState({ config });
  }
  handleReplacementAChange(event) {
    const config = this.state.config;
    config.method3 = event.target.value;
    this.setState({ config });
  }
  handleReplacementBChange(event) {
    const config = this.state.config;
    config.method4 = event.target.value;
    this.setState({ config });
  }
  handleReplacementChange(event) {
    const config = this.state.config;
    config.replacement = event.target.value;
    this.setState({ config });
  }
  handleMutationChange(event) {
    const config = this.state.config;
    config.mutationMethod = event.target.value;
    this.setState({ config });
  }
  handleMutationProbabilityChange(event) {
    const config = this.state.config;
    config.mutationProbability = event.target.value;
    this.setState({ config });
  }
  handleFinishChange(event) {
    const config = this.state.config;
    config.finish = event.target.value;
    this.setState({ config });
  }
  handleCharacterTypeChange(event) {
    const config = this.state.config;
    config.characterType = event.target.value;
    this.setState({ config });
  }
  handleGenLimitChange(event) {
    const config = this.state.config;
    config.generationsLimit = event.target.value;
    this.setState({ config });
  }
  handleTimeLimitChange(event) {
    const config = this.state.config;
    config.timeLimit = event.target.value;
    this.setState({ config });
  }
  handleTournamentMChange(event) {
    const config = this.state.config;
    config.tournamentM = event.target.value;
    this.setState({ config });
  }
  handleTargetFitnessChange(event) {
    const config = this.state.config;
    config.targetFitness = event.target.value;
    this.setState({ config });
  }
  handleStructurePercentageChange(event) {
    const config = this.state.config;
    config.structurePercentage = event.target.value;
    this.setState({ config });
  }
  handleFitnessUnchangedChange(event){
    const config = this.state.config;
    config.fitnessUnchanged = event.target.value;
    this.setState({ config });
  }
  handleHeightMutationDeltaChange(event){
    const config = this.state.config;
    config.heightMutationDelta = event.target.value;
    this.setState({ config });
  }
  startEngine(){
    const requestOptions = {
        method: 'POST',
        body: JSON.stringify(this.state.config)
    };
    this.storedAvgFitness = [];
    this.storedMaxFitness = [];
    this.storedMinFitness = [];
	this.storedDiversity = [];
    ApexCharts.exec('fitness', 'updateOptions', {
      series: [{
        name:"Fitness Promedio",
        data: this.storedAvgFitness
      },{
        name:"Fitness Maximo",
        data: this.storedMaxFitness
      },{
        name:"Fitness Minimo",
        data: this.storedMinFitness
      }],
      xaxis: {
        max:500,
        range:500
      }
    },true)
    ApexCharts.exec('diversity', 'updateOptions', {
      series: [{
		name: "Diversidad",
		data: this.storedDiversity
	  }],
      xaxis: {
        max:500,
        range:500
      }
    },true);
    fetch('http://localhost:5000/start', requestOptions)
      .then(() => {
        let intervalId = window.setInterval(() => {
          fetch('http://localhost:5000/data')
            .then(response => response.json())
            .then(data => {
			  if(!data || data.length === 0)
				return;
              if(data.length > 0 && data[data.length - 1] === null){
                  window.clearInterval(intervalId);
                  this.setState({ running: false });
                if(data.length <= 1) 
                  return;
                else 
                  data = data.slice(0,data.length - 1);
              }
              const avgFitnessValues = data.map(d => d.avgFitness).reduce((prev,next) => prev.concat(next),[]);
              const maxFitnessValues = data.map(d => d.maxFitness).reduce((prev,next) => prev.concat(next),[]);
              const minFitnessValues = data.map(d => d.minFitness).reduce((prev, next) => prev.concat(next), []);
			  const diversityValues = data.map(d => d.diversity).reduce((prev, next) => prev.concat(next), []);
              this.setState({ bestCharacter: data[data.length - 1].bestCharacter });
              this.storedAvgFitness = this.storedAvgFitness.concat(avgFitnessValues);
              this.storedMaxFitness = this.storedMaxFitness.concat(maxFitnessValues);
              this.storedMinFitness = this.storedMinFitness.concat(minFitnessValues);
              this.storedDiversity = this.storedDiversity.concat(diversityValues);
              ApexCharts.exec('fitness', 'updateSeries', [{
                  name:"Fitness Promedio",
                  data: this.storedAvgFitness
                },{
                  name:"Fitness Maximo",
                  data: this.storedMaxFitness
                },{
                  name:"Fitness Minimo",
                  data: this.storedMinFitness
              }],true);
              ApexCharts.exec('diversity', 'updateSeries', [{
                  name:"Diversidad",
                  data: this.storedDiversity
			  }],true);
              if(this.storedAvgFitness.length >= 500){
                ApexCharts.exec('fitness', 'updateOptions', {
                  xaxis:{
                        max: this.storedAvgFitness.length + 20,
                        range: this.storedAvgFitness.length + 20
                  }
                },true);
              }
			  if(this.storedDiversity.length >= 500){
                ApexCharts.exec('diversity', 'updateOptions', {
                  xaxis:{
                        max: this.storedDiversity.length + 20,
                        range: this.storedDiversity.length + 20
                  }
                },true);
			  }
            }, console.log);
          
        }, 200);
      }, console.log);
    this.setState({running:true});
  }
  
  render() {
    return (
      <div className="App">
        <div className="start-button-container">
            <button type="button" id="btn-send" className="start-button" onClick={() => {this.startEngine()}} disabled={this.state.running}>
                Start
            </button>
        </div>
        <div className="configuration-row">
            <div className="configuration-item" >
                <label className="configuration-label">Clase:</label>
                <select  onChange={(event) => this.handleCharacterTypeChange(event) } defaultValue={this.state.config.characterType}>
                    <option value="warrior">Guerrero</option>
                    <option value="archer">Arquero</option>
                    <option value="defender">Defensor</option>
                    <option value="rogue">Infiltrado</option>
                </select>
            </div>
            <div className="configuration-item" >
                <label className="configuration-label">N:</label>
                    <input type="numeric" placeholder="N" onChange={(event) => this.handleNChange(event)} value={ this.state.config.n }/>
            </div>
            <div className="configuration-item" >
                <label className="configuration-label">K:</label>
                <input type="numeric" placeholder="K" onChange={(event) => this.handleKChange(event)} value={ this.state.config.k }/>
            </div>
            <div className="configuration-item" >
                <label className="configuration-label">A:</label>
                <input type="numeric" placeholder="A" onChange={(event) => this.handleAChange(event)}  value={ this.state.config.a }/>
            </div>
            <div className="configuration-item" >
                <label className="configuration-label">B:</label>
                <input type="numeric" placeholder="B" onChange={(event) => this.handleBChange(event)} value={ this.state.config.b } />
            </div>
        </div>
		
        <div className="configuration-row">
            <div className="configuration-item" >
                <label className="configuration-label">Cruza:</label>
                <select  onChange={(event) =>  this.handleCrossoverChange(event) } defaultValue={this.state.config.crossoverMethod}>
                    <option value="one point">Un punto</option>
                    <option value="two point">Dos puntos</option>
                    <option value="anular">Anular</option>
                    <option value="uniform">Uniforme</option>
                </select>
            </div>
            <div className="configuration-item" >
                <label className="configuration-label">Seleccion A:</label>
                <select  onChange={(event) => this.handleSelectionAChange(event) } defaultValue={this.state.config.method1}>
                    <option value="elite">Elite</option>
                    <option value="roulette">Ruleta</option>
                    <option value="ranking">Ranking</option>
                    <option value="boltzmann">Boltzmann</option>
                    <option value="tournament det">Torneo deterministico</option>
                    <option value="tournament prob">Torneo Probabilistico</option>
                    <option value="universal">Universal</option>
                </select>
            </div>
            <div className="configuration-item" >
                <label className="configuration-label">Seleccion B:</label>
                <select onChange={(event) => this.handleSelectionBChange(event) } defaultValue={this.state.config.method2}>
                    <option value="elite">Elite</option>
                    <option value="roulette">Ruleta</option>
                    <option value="ranking">Ranking</option>
                    <option value="boltzmann">Boltzmann</option>
                    <option value="tournament det">Torneo deterministico</option>
                    <option value="tournament prob">Torneo Probabilistico</option>
                    <option value="universal">Universal</option>
                </select>
            </div>
            <div className="configuration-item" >
                <label className="configuration-label">Reemplazo A:</label>
                <select onChange={(event) => this.handleReplacementAChange(event) } defaultValue={this.state.config.method3}>
                    <option value="elite">Elite</option>
                    <option value="roulette">Ruleta</option>
                    <option value="ranking">Ranking</option>
                    <option value="boltzmann">Boltzmann</option>
                    <option value="tournament det">Torneo deterministico</option>
                    <option value="tournament prob">Torneo Probabilistico</option>
                    <option value="universal">Universal</option>
                </select>
            </div>
            <div className="configuration-item" >
                <label className="configuration-label">Reemplazo B:</label>
                <select  onChange={(event) => this.handleReplacementBChange(event) } defaultValue={this.state.config.method4}>
                    <option value="elite">Elite</option>
                    <option value="roulette">Ruleta</option>
                    <option value="ranking">Ranking</option>
                    <option value="boltzmann">Boltzmann</option>
                    <option value="tournament det">Torneo deterministico</option>
                    <option value="tournament prob">Torneo Probabilistico</option>
                    <option value="universal">Universal</option>
                </select>
            </div>
            <div className="configuration-item" >
                <label className="configuration-label">Implementacion</label>
                <select onChange={(event) => this.handleReplacementChange(event) } defaultValue={this.state.config.replacementMethod}>
                    <option value="fill all">Fill All</option>
                    <option value="fill parent">Fill Parent</option>
                </select>
            </div>
            <div className="configuration-item" >
                <label className="configuration-label">Mutacion</label>
                <select onChange={(event) => this.handleMutationChange(event) } defaultValue={this.state.config.mutationMethod}>
                    <option value="gene">Gen</option>
                    <option value="multigene limited">Multigen Limitada</option>
                    <option value="multigene uniform">Multigen Uniforme</option>
                    <option value="complete">Completa</option>
                </select>
            </div>
            <div className="configuration-item" >
                <label className="configuration-label">Corte</label>
                <select onChange={(event) => this.handleFinishChange(event) } defaultValue={this.state.config.finish}>
                    <option value="time">Tiempo</option>
                    <option value="generations">Generaciones</option>
                    <option value="acceptable solution">Solucion aceptable</option>
                    <option value="structure">Estructura</option>
                    <option value="content">Contenido</option>
                </select>
            </div>
        </div>
        <div className="configuration-row">
            <div className="configuration-item" >
                    <label className="configuration-label">Probabilidad de mutacion:</label>
                    <input type="numeric" placeholder="P" onChange={(event) => this.handleMutationProbabilityChange(event)} value={this.state.config.mutationProbability} />
            </div>
            <div className="configuration-item" >
                <label className="configuration-label">Rango Mutación Altura:</label>
                <input type="numeric" placeholder="delta" onChange={(event) => this.handleHeightMutationDeltaChange(event)} value={ this.state.config.heightMutationDelta }/>
            </div>
            <div className="configuration-item" >
                <label className="configuration-label">Limite de generaciones:</label>
                <input type="numeric" placeholder="Generaciones" onChange={(event) => this.handleGenLimitChange(event)}  value={ this.state.config.generationsLimit }/>
            </div>
            <div className="configuration-item" >
                <label className="configuration-label">Limite de tiempo:</label>
                <input type="numeric" placeholder="Tiempo" onChange={(event) => this.handleTimeLimitChange(event)}  value={ this.state.config.timeLimit }/>
            </div>
            <div className="configuration-item" >
                <label className="configuration-label">Torneo M:</label>
                <input type="numeric" placeholder="M" onChange={(event) => this.handleTournamentMChange(event)} value={ this.state.config.tournamentM }/>
            </div>
            <div className="configuration-item" >
                <label className="configuration-label">Fitness Aceptable:</label>
                <input type="numeric" placeholder="Fitness" onChange={(event) => this.handleTargetFitnessChange(event)} value={ this.state.config.targetFitness }/>
            </div>
            <div className="configuration-item" >
                <label className="configuration-label">Porcentaje Iguales:</label>
                <input type="numeric" placeholder="P" onChange={(event) => this.handleStructurePercentageChange(event)} value={ this.state.config.structurePercentage }/>
            </div>
            <div className="configuration-item" >
                <label className="configuration-label">Rango Max. Fitness:</label>
                <input type="numeric" placeholder="delta" onChange={(event) => this.handleFitnessUnchangedChange(event)} value={ this.state.config.fitnessUnchanged }/>
            </div>
        </div>
        {
            !this.state.running && this.state.bestCharacter && 
            <div>
                <h1 style={{ marginTop: 0,marginBottom: 0 }} >Mejor Personaje</h1>
				<div style={{display:'flex',marginTop: 14 + 'px',marginBottom: 14 + 'px',justifyContent:'center'}}>
					<b>ALTURA: </b>{this.state.bestCharacter.height.toFixed(2)} <br />
				</div>
                <div id="character-data" className="character-data">
                    <div id="helmet-data" className="character-item">
                        CASCO <br />
                        <CharacterItem item={this.state.bestCharacter.helmet} />
                    </div>
                    <div id="chest-data" className="character-item">
                        PECHERA <br />
                        <CharacterItem item={this.state.bestCharacter.chest} />
                    </div>
                    <div id="weapon-data" className="character-item">
                        ARMA <br />
                        <CharacterItem item={this.state.bestCharacter.weapon} />
                    </div>
                    <div id="gloves-data" className="character-item">
                        GUANTES <br />
                        <CharacterItem item={this.state.bestCharacter.gloves} />
                    </div>
                    <div id="boots-data" className="character-item">
                        BOTAS <br />
                        <CharacterItem item={this.state.bestCharacter.boots} />
                    </div>
                </div>
            </div>
        }
        <div id="chart" className="chart-container">
            <ReactApexChart options={this.state.fitnessChartOptions} series={this.state.fitnessSeries} type="line" height="100%" />
        </div>
        <div id="chart" className="chart-container">
            <ReactApexChart options={this.state.diversityChartOptions} series={this.state.diversitySeries} type="line" height="100%" />
        </div>
      </div>
    );
  }
}

export default App;
