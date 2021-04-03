import React from 'react';
import ReactApexChart from 'react-apexcharts';
import ApexCharts from 'apexcharts';
import './App.css';

class App extends React.Component {
  
  
  constructor(props) {
    super(props);
    this.storedAvgFitness = [];
    this.storedMinFitness = [];
    this.storedMaxFitness = [];
    this.state = {
    
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
      options: {
        chart: {
          id: 'realtime',
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
            color:  '#263238'
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
      started: false
    
    };
  }


  startEngine(){
    const requestOptions = {
      method: 'POST'
    };
    this.storedAvgFitness = [];
    this.storedMaxFitness = [];
    this.storedMinFitness = [];
    ApexCharts.exec('realtime', 'updateOptions', {
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
        max:500
      }
    },true);
    fetch('http://localhost:5000/start', requestOptions)
      .then(() => {
        let intervalId = window.setInterval(() => {
          fetch('http://localhost:5000/data')
            .then(response => response.json())
            .then(data => {
              if(data.length > 0 && data[data.length - 1] === null){
                window.clearInterval(intervalId);
                this.setState({started:false});
                console.log(data);
                if(data.length === 1) 
                  return;
                else 
                  data = data.slice(0,data.length - 2);
              }
              const avgFitnessValues = data.map(d => d.avgFitness).reduce((prev,next) => prev.concat(next),[]);
              const maxFitnessValues = data.map(d => d.maxFitness).reduce((prev,next) => prev.concat(next),[]);
              const minFitnessValues = data.map(d => d.minFitness).reduce((prev,next) => prev.concat(next),[]);
              this.storedAvgFitness = this.storedAvgFitness.concat(avgFitnessValues);
              this.storedMaxFitness = this.storedMaxFitness.concat(maxFitnessValues);
              this.storedMinFitness = this.storedMinFitness.concat(minFitnessValues);

              ApexCharts.exec('realtime', 'updateSeries', [{
                  name:"Fitness Promedio",
                  data: this.storedAvgFitness
                },{
                  name:"Fitness Maximo",
                  data: this.storedMaxFitness
                },{
                  name:"Fitness Minimo",
                  data: this.storedMinFitness
              }],true);
              if((this.storedAvgFitness.length) >= 500){
                ApexCharts.exec('realtime', 'updateOptions', {
                  xaxis:{
                    max:this.storedAvgFitness.length + 20
                  }
                },true);
              }
            }, console.log);
          
        }, 200);
      }, console.log);
    this.setState({started:true});
  }


  render() {
    return (
      <div className="App">
        <div className="start-button-container">
            <button type="button" id="btn-send" className="start-button" onClick={() => {this.startEngine()}} disabled={this.state.started}>
                Start
            </button>
        </div>
        <div id="chart" className="chart-container">
          <ReactApexChart options={this.state.options} series={this.state.series} type="line" height="100%" />
        </div>
      </div>
    );
  }
}

export default App;
