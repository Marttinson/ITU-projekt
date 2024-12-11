using ITU_projekt.Models;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace ITU_projekt.ViewModels;

// TODO initializovat s x range = point count
// x 0 (posledni hodnota) v pravym rohu
// zleva orezavat podle pozadavku uzivatele


public class GraphViewModel : INotifyPropertyChanged
{
    private double _xAxisMin;
    private double _xAxisMax;
    private int _xAxisRange;
    private bool _showAll;
    private PlotModel _plotModel;
    private ObservableCollection<DataPoint> _dataPoints;

    public GraphViewModel(UnitModel model)
    {
        _xAxisRange = 10; // Default to show latest 10 points
        _showAll = false;
        _dataPoints = new ObservableCollection<DataPoint>();
        _plotModel = CreatePlotModel();
        GenerateDataPoints(model);

        ToggleRangeCommand = new RelayCommand((obj) => ToggleRange());
        IncrementRangeCommand = new RelayCommand((obj) => IncrementRange());
        DecrementRangeCommand = new RelayCommand((obj) => DecrementRange());
    }

    public PlotModel PlotModel
    {
        get => _plotModel;
        set
        {
            _plotModel = value;
            OnPropertyChanged();
        }
    }

    public int XAxisRange
    {
        get => _xAxisRange;
        set
        {
            _xAxisRange = value;
            OnPropertyChanged();
            UpdatePlotRange();
        }
    }

    public bool ShowAll
    {
        get => _showAll;
        set
        {
            _showAll = value;
            OnPropertyChanged();
            UpdatePlotRange();
        }
    }

    public ICommand ToggleRangeCommand { get; }
    public ICommand IncrementRangeCommand { get; }
    public ICommand DecrementRangeCommand { get; }

    private void ToggleRange()
    {
        ShowAll = !ShowAll;
    }

    private void IncrementRange()
    {
        XAxisRange++;
        if (XAxisRange > _dataPoints.Count)
        {
            XAxisRange = _dataPoints.Count;
        }
    }

    private void DecrementRange()
    {
        if (XAxisRange > 1)
        {
            XAxisRange--;
        }
    }

    private void UpdatePlotRange()
    {
        var xAxis = _plotModel.Axes[0] as LinearAxis;
        if (xAxis != null)
        {
            // Ensure X-Axis always starts at 0 and goes up to the count of data points
            xAxis.Minimum = 0;
            xAxis.Maximum = _dataPoints.Count;

            // Adjust the visible range based on ShowAll and XAxisRange
            if (ShowAll)
            {
                xAxis.Minimum = 0;
                xAxis.Maximum = _dataPoints.Count;
            }
            else
            {
                xAxis.Minimum = Math.Max(0, _dataPoints.Count - XAxisRange);
                xAxis.Maximum = _dataPoints.Count;
            }

            _plotModel.InvalidatePlot(true);
        }
    }

    private PlotModel CreatePlotModel()
    {
        var model = new PlotModel { Title = "Sample Graph" };

        var xAxis = new LinearAxis
        {
            Position = AxisPosition.Bottom,
            Title = "X-Axis"
        };

        var yAxis = new LinearAxis
        {
            Position = AxisPosition.Left,
            Title = "Y-Axis"
        };

        yAxis.Minimum = 0;
        yAxis.Maximum = 1;

        model.Axes.Add(xAxis);
        model.Axes.Add(yAxis);

        // Create ScatterSeries instead of LineSeries
        var scatterSeries = new ScatterSeries
        {
            MarkerType = MarkerType.Circle, // Set marker type (Circle, Square, etc.)
            MarkerSize = 6, // Set marker size (data points)
            MarkerFill = OxyColors.Red // Set color of markers (data points)
        };
        model.Series.Add(scatterSeries);

        return model;
    }

    private void GenerateDataPoints(UnitModel model)
    {
        if (model?.ErrorRates == null || !model.ErrorRates.Any()) return;

        // Create the points for the ScatterSeries
        var scatterSeries = (ScatterSeries)_plotModel.Series[0];

        // Reverse the points order to have the latest data on the right
        for (int i = 0; i < model.ErrorRates.Count; i++)
        {
            scatterSeries.Points.Add(new ScatterPoint(model.ErrorRates.Count - 1 - i, model.ErrorRates[i])); // Reverse the order of data points
        }

        UpdatePlotRange();
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
