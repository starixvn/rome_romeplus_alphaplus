"use strict"

class TurnoverBySportChart {
    private chart: any;
    private canvasId: string;
    private isDarkMode: boolean;

    constructor(isDarkMode = false, canvasId = 'canvas-turnover-by-sport-chart') {
        this.canvasId = canvasId;
        this.isDarkMode = isDarkMode;
    }

    draw(sportTypes: string[], turnovers: number[]) {
        if (this.chart) {
            this.redraw(sportTypes, turnovers);
            return;
        }

        const canvas = document.getElementById(this.canvasId);
        const chartTitle = canvas.getAttribute('aria-label');

        const data = {
            labels: sportTypes,
            datasets: [
                {
                    data: turnovers,
                    backgroundColor: [
                        'rgb(46, 131, 204)',
                        'rgb(242, 99, 80)',
                        'rgb(154, 114, 176)',
                        'rgb(164, 164, 164)',
                        'rgb(75, 192, 192)',
                    ]
                }
            ]
        };

        const darkTextColor = "#EDEDED";
        const config = {
            type: 'doughnut',
            data: data,
            options: {
                plugins: {
                    title: {
                        text: chartTitle,
                        padding: {
                            bottom: 25
                        },
                        color: this.isDarkMode && darkTextColor,
                    },
                    legend: {
                        title: {
                            display: true,
                            padding: {
                                bottom: 2
                            }
                        },
                        labels: {
                            color: this.isDarkMode && darkTextColor,
                            usePointStyle: true,
                        },
                        onClick: (e) => e.stopPropagation()
                    },
                    datalabels: {
                        color: this.isDarkMode ? '#EDEDED' : '#333',
                        font: {
                            weight: 'bold',
                            size: '12px'
                        },
                        formatter: (value, ctx) => this.calculatePercentage(ctx, value) >= 1 ? `${this.calculatePercentage(ctx, value)}%` : '',
                        display: "auto",
                        labels: {
                            index: {
                                anchor: (ctx) => this.getPercentagePosition(ctx),
                                align: (ctx) => this.getPercentagePosition(ctx)
                            }
                        }
                    }
                }
            },
            plugins: [ChartDataLabels]
        };

        this.chart = new Chart(canvas, config);
    }

    redraw(sportTypes: string[], turnovers: number[]) {
        this.chart.data.labels = sportTypes
        this.chart.data.datasets[0].data = turnovers;
        this.chart.update();
    }

    private getPercentagePosition(ctx) {
        return 'end';
    }

    private calculatePercentage(ctx, value: number) {
        let sum = 0;
        const dataArray = ctx.chart.data.datasets[0].data;
        dataArray.map(data => sum += data);

        return Math.round((value * 100 * 100) / sum) / 100;
    }
}
