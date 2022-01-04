CREATE PROCEDURE [dbo].[sp_CreateDebt]
	@ContractId int,
	@PersonId int,
	@Date datetime, 
	@Type nvarchar(50), 
	@Amount  decimal
AS
	declare @debtId int;
	INSERT INTO Debts (ContractId, PersonId) 
	VALUES(@ContractId, @PersonId)
	SET @debtId = scope_identity()
	
	INSERT INTO Transactions (DebtId, Date, Type, Amount) 
	VALUES(@debtId, @Date, @Type, @Amount)

	
RETURN 0
