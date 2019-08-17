
CREATE FUNCTION dbo.getPropertyForProduct(@idProduct INT, @idCategory INT)
RETURNS @result TABLE (nameID INT, [name] NVARCHAR(20), valueID INT, [value] NVARCHAR(50))
AS
BEGIN
	WITH hierarchyCategories (id, idParent)
	AS
	(
		SELECT c.id, c.idParent     
		FROM dbo.categories c
		WHERE c.id = @idCategory
		UNION ALL
		SELECT c.id, c.idParent   
		FROM dbo.categories c    
			INNER JOIN hierarchyCategories hc ON ( c.id = hc.idParent )
	)

	INSERT INTO @result(nameID, [name], valueID, [value])
	SELECT pm.id nameID, pm.title [name], pv.id valueID, pv.value [value]
	FROM dbo.PropertyProducts pp
		INNER JOIN dbo.PropertyValues pv ON ( pp.idPropertyValue = pv.id )
		RIGHT JOIN dbo.PropertyNames pm ON ( pp.idPropertyName = pm.id )
			INNER JOIN hierarchyCategories hc ON ( pm.idCategory = hc.id )
	WHERE pp.idProduct = @idProduct
RETURN
END