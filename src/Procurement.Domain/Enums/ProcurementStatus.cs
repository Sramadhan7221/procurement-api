namespace Procurement.Domain.Enums;

public enum ProcurementStatus
{
    RequestCreated = 1,
    ApproveByManager = 2,
    RejectByManager = 3,
    ApproveByAdmin = 4,
    RejectByAdmin = 5,
    InOrderByAdmin = 6,
    OrderReceived = 7,
    Completed = 8
}
