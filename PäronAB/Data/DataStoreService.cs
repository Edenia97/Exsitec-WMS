
public class DataStoreService
{
    public List<Product> Products { get; set; } = new List<Product>();
    public List<Warehouse> Warehouses { get; set; } = new List<Warehouse>();
    public List<WarehouseTransaction> WarehouseTransactions { get; set; } = new List<WarehouseTransaction>();
    public List<InventoryBalance> Balances { get; set; } = new List<InventoryBalance>();
    public string? ErrorMessage { get; set; }

    public List<Product> GetProducts()
    {
        return Products;
    }

    public Product? GetProductById(int id)
    {
        return Products.FirstOrDefault(p => p.Id == id);
    }

    public void AddProduct(Product product)
    {
        product.Id = Products.Count > 0 ? Products.Max(p => p.Id) + 1 : 1;
        Products.Add(product);
    }

    public void DeleteProduct(int id)
    {
       var product = GetProductById(id);
       if (product != null) 
       {
         Products.Remove(product);
       }
    }

    public List<Warehouse> GetWarehouses()
    {
       return Warehouses;
    }
     
    public Warehouse? GetWarehouseById( int id)
    {
       return Warehouses.FirstOrDefault(p => p.Id == id);
    }
    public void AddWarehouse (Warehouse warehouse)
    {
        warehouse.Id = Warehouses.Count > 0 ? Warehouses.Max(p => p.Id) + 1 : 1;
        Warehouses.Add(warehouse);
    }

    public void DeleteWarehouse( int id)
    {
      var warehouse = GetWarehouseById(id);
      if(warehouse != null)
      {
        Warehouses.Remove(warehouse);
      }
    }

    public List<WarehouseTransaction> GetWarehouseTransactions()
    {
        return WarehouseTransactions;
    }

    public WarehouseTransaction? GetWarehouseTransactionById(int id)
    {
        return WarehouseTransactions.FirstOrDefault(t => t.Id == id);
    }
    public void RemoveWarehouseTransaction(int id)
    {
        var transaction = GetWarehouseTransactionById(id);
        if (transaction != null)
        {
            WarehouseTransactions.Remove(transaction);
        }
    }

    public InventoryBalance? GetBalanceById(int id)
    {
        return Balances.FirstOrDefault(b => b.Id == id);
    }
    public void RemoveBalance(int id)
    {
        var balance = GetBalanceById(id);
        if (balance != null)
        {
            Balances.Remove(balance);
        }
    }
    public void AddWarehouseTransaction(WarehouseTransaction transaction)
    {
        // Anta att Id är unikt och auto-increment
        transaction.Id = WarehouseTransactions.Count > 0 ? WarehouseTransactions.Max(t => t.Id) + 1 : 1;
        WarehouseTransactions.Add(transaction);
        CalculateInventoryBalance();
        List<InventoryBalance> balancesToRemove = new List<InventoryBalance>();
        foreach (var balance in Balances)
        {
          if(balance.Balance < 0)
          {
                RemoveWarehouseTransaction(transaction.Id);
                balancesToRemove.Add(balance);
                //måste ta bort den negativa balance också
                ErrorMessage = " The transaction was denied, the inventory balance is not sifucient to make this transaction.";
          }else{
                ErrorMessage = "";
            }
        }
        foreach (var balance in balancesToRemove)
        {
            RemoveBalance(balance.Id);
        }
    }

    public void CalculateInventoryBalance()
    {
        Balances.Clear();

        foreach ( var transaction in WarehouseTransactions)
        {   
            //find the balance for a specific product in a specific warehouse 
            var inventoryBalance = Balances.FirstOrDefault(b => b.ProductId == transaction.ProductId && b.WarehouseId == transaction.WarehouseId);
            //Kommer det inte alltid vara null?
            if (inventoryBalance == null)
            {
                inventoryBalance = new InventoryBalance
                {
                   Id = Balances.Count > 0 ? Balances.Max(p => p.Id) + 1 : 1,
                   ProductId = transaction.ProductId,
                   WarehouseId = transaction.WarehouseId,
                   Balance = 0
                };
                Balances.Add(inventoryBalance);
            }

            if (transaction.Type == WarehouseTransaction.TransactionType.Ingoing)
            {
               inventoryBalance. Balance += transaction.Amount;
            }
            else if (transaction.Type == WarehouseTransaction.TransactionType.Outgoing)
            {
                inventoryBalance.Balance -= transaction.Amount;
            } 
        }
    }
    
    public List<InventoryBalance> GetBalances()
    {
        return Balances;
    }
}   