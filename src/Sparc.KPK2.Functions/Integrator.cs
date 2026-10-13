using System.Collections.Generic;

namespace Sparc.KPK2.NFunctions
{
  public class Integrator //интегратор, производит усреднение значений по каналам с заданной глубиной на канале
  {
    private readonly Dictionary<int, List<double>> _buffer;
    private readonly Dictionary<int, int> _depth;
    private int _defaultDepth = 1;

    public int DefaultDepth
    {
      get { return _defaultDepth; }
      set { _defaultDepth = value; }
    }

    public Integrator()
    {
      _buffer = new Dictionary<int, List<double>>();
      _depth = new Dictionary<int, int>();
    }

    public Integrator(int defaultDepth)
    {
      _buffer = new Dictionary<int, List<double>>();
      _depth = new Dictionary<int, int>();
      _defaultDepth = defaultDepth;
    }
    /// <summary>
    /// Добавить значение для интегрирования
    /// </summary>
    /// <param name="channel"></param>
    /// <param name="value"></param>
    public void AddValue(int channel, double value)
    {
      if (!_buffer.ContainsKey(channel))
      {
        _buffer.Add(channel, new List<double>());
        if (!_depth.ContainsKey(channel))
          SetDepth(channel, _defaultDepth);
      }
      _buffer[channel].Add(value);
      if (_buffer[channel].Count > _depth[channel])
        _buffer[channel].RemoveAt(0);
    }

    /// <summary>
    /// Получить интегрированное значение
    /// </summary>
    /// <param name="channel"></param>
    /// <returns></returns>
    public double GetValue(int channel)
    {
      if (!_buffer.ContainsKey(channel))
        return 0;
      if (_buffer[channel] == null)
        return 0;
      double intg = 0;
      int count = 0;
      foreach (double d in _buffer[channel])
      {
        intg += d;
        count++;
      }
      intg /= count;
      return intg;
    }

      public double GetLastIncValue(int channel)
      {
          if (!_buffer.ContainsKey(channel))
              return 0;
          if (_buffer[channel] == null)
              return 0;
          if (_buffer[channel].Count == 0)
              return 0;
          return _buffer[channel][_buffer[channel].Count-1];
      }

    /// <summary>
    /// Установка глубины интегрирования на канале
    /// </summary>
    /// <param name="channel"></param>
    /// <param name="value"></param>
    public void SetDepth(int channel, int value)
    {
      if (!_depth.ContainsKey(channel))
        _depth.Add(channel, value);
      else
      {
        _depth[channel] = value;
      }
    }

    /// <summary>
    /// Очистка всех каналов от значений
    /// </summary>
    public void Clear()
    {
        foreach (var item in _buffer)
            item.Value.Clear();
    }

    public void ClearToLastValue(int channel)
    {
        if (!_buffer.ContainsKey(channel))
            return;
        if (_buffer[channel] == null)
            return;
        if (_buffer[channel].Count == 0)
            return;
        var lastValue = _buffer[channel][_buffer[channel].Count - 1];
        _buffer[channel].Clear();
        _buffer[channel].Add(lastValue);
    }
  }
}
