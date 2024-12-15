/* GraphViewModel
 * VM
 * Vojtěch Hrabovský (xhrabo18)
 * 
 * VM - Fills graph with data, handles controls
 */

using System.ComponentModel;
using System.Windows.Input;
using System;

using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot;

using ITU_projekt.Models;
public class GraphViewModel : INotifyPropertyChanged
{
    // x axis range (how many latest samples will be shown)
    private int _xAxisRange;
    // toggle to show all samples
    private bool _showAll;
    // Plot model that will be shown
    private PlotModel _plotModel;
    // Unit model with statistics
    private UnitModel _model;

    public PlotModel PlotModel
    {
        get => _plotModel;
        set
        {
            _plotModel = value;
            OnPropertyChanged();
        }
    }

    // How many datapoint will be shown (from the end)
    public int XAxisRange
    {
        get => _xAxisRange;
        set
        {
            _xAxisRange = value;
            OnPropertyChanged();
            GenerateDataPoints(); // Recalculate points on range change
        }
    }

    // Show all data points toggle
    public bool ShowAll
    {
        get => _showAll;
        set
        {
            _showAll = value;
            OnPropertyChanged();
            GenerateDataPoints(); // Recalculate points when toggling range
        }
    }

    // Commands for controls
    public ICommand ToggleRangeCommand { get; }
    public ICommand IncrementRangeCommand { get; }
    public ICommand DecrementRangeCommand { get; }

    /// <summary>
    /// Initializes instance
    /// </summary>
    /// <param name="model">UnitModel with datapoints to show</param>
    public GraphViewModel(UnitModel model)
    {
        _xAxisRange = model.ErrorRates.Count / 2; // Default range
        _showAll = false;
        _model = model; // Unit to display

        _plotModel = CreatePlotModel();
        GenerateDataPoints();

        ToggleRangeCommand = new RelayCommand((obj) => ToggleRange());
        IncrementRangeCommand = new RelayCommand((obj) => IncrementRange());
        DecrementRangeCommand = new RelayCommand((obj) => DecrementRange());
    }


    // Toggle show all
    private void ToggleRange()
    {
        ShowAll = !ShowAll;
    }

    // Increment range
    private void IncrementRange()
    {
        XAxisRange++;
        if (_model?.ErrorRates != null && XAxisRange > _model.ErrorRates.Count)
        {
            XAxisRange = _model.ErrorRates.Count;
        }
    }

    // Decrement range
    private void DecrementRange()
    {
        if (XAxisRange > 1)
        {
            XAxisRange--;
        }
    }

    // Create plot model for graph
    private PlotModel CreatePlotModel()
    {
        // title
        var model = new PlotModel { Title = "Procentuální míra neúspěšnosti posledních x pokusů" };

        // Create X-axis
        var xAxis = new LinearAxis
        {
            Position = AxisPosition.Bottom,
            IsAxisVisible = false // Hide the X-axis
        };
        model.Axes.Add(xAxis);

        // Create Y-axis
        var yAxis = new LinearAxis
        {
            Position = AxisPosition.Left,
            Title = "Míra neúspěchu",
            Minimum = 0,
            Maximum = 1
        };
        model.Axes.Add(yAxis);

        // ScatterSeries
        var scatterSeries = new ScatterSeries
        {
            MarkerType = MarkerType.Circle,
            MarkerSize = 4,
            MarkerFill = OxyColors.Red
        };
        model.Series.Add(scatterSeries);

        return model;
    }

    // Generate data points from unit model statistic
    private void GenerateDataPoints()
    {
        if (_model?.ErrorRates == null || _model.ErrorRates.Count == 0) return;

        // Clear previous points
        var scatterSeries = (ScatterSeries)_plotModel.Series[0];
        scatterSeries.Points.Clear();

        int totalPoints = _model.ErrorRates.Count;

        // Determine the range of points to display
        int pointsToDisplay = ShowAll ? totalPoints : Math.Min(XAxisRange, totalPoints);

        // Show last x elements
        int startIndex = totalPoints - pointsToDisplay;

        // Recalculate points with adjusted X-coordinates
        for (int i = 0; i < pointsToDisplay; i++)
        {
            int xValue = i; // X-values must start at 0 and increment
            double yValue = _model.ErrorRates[startIndex + i];

            scatterSeries.Points.Add(new ScatterPoint(xValue, yValue));
        }

        UpdatePlotRange(pointsToDisplay);
    }

    // Update plot x axis range
    private void UpdatePlotRange(int pointsToDisplay)
    {
        var xAxis = _plotModel.Axes[0] as LinearAxis;
        if (xAxis != null)
        {
            xAxis.Minimum = 0; // Start from 0
            xAxis.Maximum = pointsToDisplay; // End at the last point's X-coordinate
        }

        _plotModel.InvalidatePlot(true); // Refresh the graph
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
