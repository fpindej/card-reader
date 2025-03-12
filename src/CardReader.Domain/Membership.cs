using System;

namespace CardReader.Domain;

public class Membership
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsActive { get; set; }

    public void Activate()
    {
        IsActive = true;
    }

    public void Deactivate()
    {
        IsActive = false;
    }

    public void Extend(int days)
    {
        EndDate = EndDate.AddDays(days);
    }

    public void Revoke()
    {
        IsActive = false;
        EndDate = DateTime.Now;
    }
}
