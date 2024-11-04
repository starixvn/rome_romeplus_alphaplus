"use strict"

class TurnoverWinLossByDateChart {
    private chart: any;
    private canvasId: string;
    private isDarkMode: boolean;

    constructor(isDarkMode = false , canvasId = 'canvas-turnover-winloss-by-date-chart') {
        this.canvasId = canvasId;
        this.isDarkMode = isDarkMode;
    }

    draw(dates: string[], turnovers: number[], winloss: number[]) {
        if (this.chart) {
            this.redraw(dates, turnovers, winloss);
            return;
        }

        const canvas = document.getElementById(this.canvasId);
        const chartTitle = canvas.getAttribute('aria-label');

        const data = {
            labels: dates,
            datasets: [
                {
                    label: canvas.getAttribute('data-turnover'),
                    data: turnovers,
                    borderColor: 'rgb(54, 162, 235)',
                    backgroundColor: 'rgb(54, 162, 235, 0.5)',
                    fill: true,
                    yAxisID: 'y',
                    pointStyle: false
                    //order: 2
                },
                {
                    label: canvas.getAttribute('data-winloss'),
                    data: winloss,
                    borderColor: 'rgb(255, 159, 64)',
                    yAxisID: 'y1',
                    pointStyle: false
                    //order: 1 // this dataset is drawn on top
                }
            ]
        };

        const darkTextColor = "#EDEDED";
        const config = {
            type: 'line',
            data: data,
            options: {
                responsive: true,
                maintainAspectRatio: false,
                interaction: {
                    mode: 'index',
                    intersect: false
                },
                elements: {
                    line: {
                        borderWidth: 1.5
                    }
                },
                plugins: {
                    title: {
                        text: chartTitle,
                        color: this.isDarkMode && darkTextColor
                    },
                    legend: {
                        labels: {
                            color: this.isDarkMode && darkTextColor
                        },
                        onClick: (e) => e.stopPropagation()
                    },
                },
                scales: {
                    x: {
                        grid: {
                            display: false
                        },
                        ticks: {
                            color: this.isDarkMode && darkTextColor,
                        }
                    },
                    y: {
                        title: {
                            display: true,
                            text: canvas.getAttribute('data-turnover'),
                            font: {
                                size: 14,
                                weight: 'bold'
                            },
                            color: this.isDarkMode && darkTextColor
                        },
                        position: 'left',
                        ticks: {
                            callback: (value) => SiteHelper.FormatCompactNumber(value),
                            color: this.isDarkMode && darkTextColor,
                        },
                        grid: {
                            display: false
                        }
                    },
                    y1: {
                        title: {
                            display: true,
                            text: canvas.getAttribute('data-winloss'),
                            font: {
                                size: 14,
                                weight: 'bold'
                            },
                            color: this.isDarkMode && darkTextColor
                        },
                        position: 'right',
                        ticks: {
                            callback: (value) => SiteHelper.FormatCompactNumber(value),
                            color: this.isDarkMode && darkTextColor,
                        },
                        grid: {
                            display: false
                        }
                    }
                }
            }
        };

        canvas["height"] = 100;
        this.chart = new Chart(canvas, config);
    }

    redraw(dates: string[], turnovers: number[], winloss: number[]) {
        this.chart.data.labels = dates
        this.chart.data.datasets[0].data = turnovers;
        this.chart.data.datasets[1].data = winloss;
        this.chart.update();
    }
}