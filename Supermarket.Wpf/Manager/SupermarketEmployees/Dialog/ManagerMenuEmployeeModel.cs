using Supermarket.Core.Domain.Auth.LoggedEmployees;
using Supermarket.Core.Domain.Employees.Roles;
using Supermarket.Core.UseCases.Admin;
using Supermarket.Core.UseCases.ManagerMenu;

namespace Supermarket.Wpf.Manager.SupermarketEmployees.Dialog;

public class ManagerMenuEmployeeModel : NotifyPropertyChangedBase
{
    private string? _name;
    private string? _surname;
    private string? _login;
    private DateTime? _hireDate;
    private int? _managerId;
    private bool _isCashier;
    private bool _isGoodsKeeper;
    private bool _isManager;

    public int Id { get; set; }
    
    public string? Name
    {
        get => _name;
        set => SetProperty(ref _name, value);
    }

    public string? Surname
    {
        get => _surname;
        set => SetProperty(ref _surname, value);
    }

    public string? Login
    {
        get => _login;
        set => SetProperty(ref _login, value);
    }

    public DateTime? HireDate
    {
        get => _hireDate;
        set => SetProperty(ref _hireDate, value);
    }

    public int? ManagerId
    {
        get => _managerId;
        set => SetProperty(ref _managerId, value);
    }

    public bool IsCashier
    {
        get => _isCashier;
        set => SetProperty(ref _isCashier, value);
    }

    public bool IsGoodsKeeper
    {
        get => _isGoodsKeeper;
        set => SetProperty(ref _isGoodsKeeper, value);
    }

    public bool IsManager
    {
        get => _isManager;
        set
        {
            SetProperty(ref _isManager, value);
            if (value)
            {
                IsCashier = true;
                IsGoodsKeeper = true;
            }
        }
    }

    public static ManagerMenuEmployeeModel FromManagerMenuEmployeeDetail(ManagerMenuEmployeeDetail managerMenuEmployeeDetail)
    {
        var model = new ManagerMenuEmployeeModel()
        {
            Id = managerMenuEmployeeDetail.Id,
            Name = managerMenuEmployeeDetail.Name,
            Surname = managerMenuEmployeeDetail.Surname,
            Login = managerMenuEmployeeDetail.Login,
            HireDate = managerMenuEmployeeDetail.HireDate,
        };
        
        if (managerMenuEmployeeDetail.RoleInfo is SupermarketEmployee supermarketEmployee)
        {
            model.IsCashier = supermarketEmployee.Roles.Contains(SupermarketEmployeeRole.Cashier);
            model.IsGoodsKeeper = supermarketEmployee.Roles.Contains(SupermarketEmployeeRole.GoodsKeeper);
            model.IsManager = supermarketEmployee.Roles.Contains(SupermarketEmployeeRole.Manager);
            model.ManagerId = supermarketEmployee.ManagerId;
        }
        else
        {
            throw new UiInconsistencyException("Employee is not supermarket employee");
        }

        return model;
    }
    
    public IEmployeeRoleInfo GetEmployeeRoleInfo(int supermarketId)
    {
        return new SupermarketEmployee(supermarketId, ManagerId, GetRoles());
    }
    
    private HashSet<SupermarketEmployeeRole> GetRoles()
    {
        var roles = new HashSet<SupermarketEmployeeRole>();
        if (IsCashier)
        {
            roles.Add(SupermarketEmployeeRole.Cashier);
        }

        if (IsGoodsKeeper)
        {
            roles.Add(SupermarketEmployeeRole.GoodsKeeper);
        }

        if (IsManager)
        {
            roles.Add(SupermarketEmployeeRole.Manager);
        }

        return roles;
    }
}