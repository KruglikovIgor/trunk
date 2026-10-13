namespace KPK2.Common.MeasureVoltage
{
  /// <summary>
  /// Интерфейс подключенного источника питания
  /// </summary>
  public  interface IPowerSupply
  {
    /// <summary>
    /// Подключение источника питания
    /// </summary>
    void Connect();

    /// <summary>
    /// Установить напряжение
    /// </summary>
    /// <param name="channel"></param>
    /// <param name="voltage"></param>
    void SetVoltage(double voltage);


    /// <summary>
    /// Установить ограничение по току
    /// </summary>
    /// <param name="channel"></param>
    /// <param name="current"></param>
    void SetCurrent(double current);
        
    /// <summary>
    /// Отправка команды
    /// </summary>
    /// <param name="command"></param>
    void SendCommand(string command);

    /// <summary>
    /// установка выходного состояния канала источника питания
    /// </summary>
    /// <param name="state"></param>
    void SetOutState(bool state);
        double MaxVoltage();
        void SetAddVoltage(double setVoltage);
  }
}