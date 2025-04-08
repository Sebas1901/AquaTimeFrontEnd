// Clase de respuesta del API de Hidratación
public class HidratacionResponse
{
    public double MetaDiaria { get; set; }
    public double Consumido { get; set; }
    public double Restante { get; set; }
    public bool Resultado { get; set; }
    public List<HidratacionErrorDetail> Error { get; set; }  // Renombrada de 'Error' a 'HidratacionError'
}

// Clase de detalle de errores, renombrada para evitar conflictos
public class HidratacionErrorDetail
{
    public int ErrorCode { get; set; }
    public string Message { get; set; }
}
