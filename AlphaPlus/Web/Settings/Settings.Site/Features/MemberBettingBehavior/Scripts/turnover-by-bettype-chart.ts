"use strict"

class TurnoverByBetTypeChart {
    private chart: any;
    private canvasId: string;
    private isDarkMode: boolean;

    constructor(isDarkMode = false, canvasId = 'canvas-turnover-by-bettype-chart') {
        this.canvasId = canvasId;
        this.isDarkMode = isDarkMode;
    }

    draw(betTypes: string[], lives: number[], nonLives: number[], mixes: number[]) {
        if (this.chart) {
            this.redraw(betTypes, lives, nonLives, mixes);
            return;
        }

        const canvas = document.getElementById(this.canvasId);
        const chartTitle = canvas.getAttribute('aria-label');
        const language = canvas.getAttribute('data-language') || 'en-US';

        const data = {
            labels: betTypes,
            datasets: [
                {
                    label: canvas.getAttribute('data-live'),
                    data: lives,
                    backgroundColor: 'rgb(46, 131, 204)'
                },
                {
                    label: canvas.getAttribute('data-nonlive'),
                    data: nonLives,
                    backgroundColor: 'rgb(242, 99, 80)'
                },
                {
                    label: canvas.getAttribute('data-mix'),
                    data: mixes,
                    backgroundColor: 'rgb(164, 164, 164)'
                }
            ]
        };

        const darkTextColor = "#EDEDED";
        const config = {
            type: 'bar',
            data: data,
            options: {
                barThickness: 50,
                plugins: {
                    title: {
                        text: chartTitle,
                        padding: {
                            bottom: 30
                        },
                        color: this.isDarkMode && darkTextColor,
                    },
                    legend: {
                        labels: {
                            color: this.isDarkMode && darkTextColor,
                            usePointStyle: true,
                        },
                        onClick: (e) => e.stopPropagation()
                    },
                    tooltip: {
                        callbacks: {
                            title: function (context) {
                                return context[0].label.split(",")[0];
                            }
                        }
                    }
                },
                scales: {
                    x: {
                        stacked: true,
                        grid: {
                            display: false
                        },
                        ticks: {
                            color: this.isDarkMode && darkTextColor,
                            callback: function (value) {
                                const labelArray = this.getLabelForValue(value as number);
                                if (Array.isArray(labelArray) && labelArray.length === 2) {
                                    const maxLabelLength = language === 'zh-TW' || language === 'zh-CN' ? 7 : 15;
                                    return [
                                        labelArray[0].length > maxLabelLength ? `${labelArray[0].substring(0, maxLabelLength)}...` : labelArray[0],
                                        labelArray[1]
                                    ];
                                }

                                return labelArray;
                            }
                        }
                    },
                    y: {
                        stacked: true,
                        ticks: {
                            callback: (value) => SiteHelper.FormatCompactNumber(value),
                            color: this.isDarkMode && darkTextColor,
                        },
                        grid: {
                            display: false,
                            color: this.isDarkMode ? "#2F2F2F" : "#999"
                        }
                    }
                }
            }
        };

        this.chart = new Chart(canvas, config);
    }

    redraw(betTypes: string[], lives: number[], nonLives: number[], mixes: number[]) {
        this.chart.data.labels = betTypes
        this.chart.data.datasets[0].data = lives;
        this.chart.data.datasets[1].data = nonLives;
        this.chart.data.datasets[2].data = mixes;
        this.chart.update();
    }
}