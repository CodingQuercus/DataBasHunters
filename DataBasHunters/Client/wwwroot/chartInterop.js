function createChart(chartElement, chartType, data, options) {
    console.log("Chart Element:", chartElement);
    console.log("Chart Type:", chartType);
    console.log("Data:", data);
    console.log("Options:", options);

    if (chartElement) {
        var ctx = chartElement.getContext('2d');
        return new Chart(ctx, {
            type: chartType,
            data: data,
            options: options
        });
    } else {
        console.error('Canvas element not found');
    }
}
