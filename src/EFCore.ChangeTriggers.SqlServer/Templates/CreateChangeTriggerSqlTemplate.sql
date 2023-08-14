{{- func format_sql_identifier(identifier, alias = "")
	if alias != ""
		alias = "[" + alias + "]."
	end

	ret alias + "[" + identifier + "]"
end -}}

{{- func build_join(col)
	ret format_sql_identifier(col, "d") + " = " + format_sql_identifier(col, "i")
end -}}

{{- format_inserted_column(col) = format_sql_identifier(col, "i") -}}

{{- format_deleted_column(col) = format_sql_identifier(col, "d") -}}

/*
Auto-generated trigger by EFCore.ChangeTriggers
https://github.com/codemunkie15/EFCore.ChangeTriggers
*/

CREATE TRIGGER [dbo].[{{trigger_name}}]
ON [dbo].[{{tracked_table_name}}]
FOR INSERT, UPDATE, DELETE
NOT FOR REPLICATION
AS
BEGIN
	SET NOCOUNT ON

	DECLARE @OperationTypeId {{string.upcase operation_type_column.type}}
	{{~ if change_source_column != null ~}}
	DECLARE @ChangeSource {{string.upcase change_source_column.type}}
	{{~ end ~}}
	{{~ if changed_by_column != null ~}}
	DECLARE @ChangedBy {{string.upcase changed_by_column.type}}
	{{~ end ~}}

	SET @OperationTypeId =
		(CASE
			WHEN EXISTS(SELECT * FROM INSERTED) AND EXISTS(SELECT * FROM DELETED)
				THEN 2  -- UPDATE
			WHEN EXISTS(SELECT * FROM INSERTED)
				THEN 1  -- INSERT
			WHEN EXISTS(SELECT * FROM DELETED)
				THEN 3  -- DELETE
		END)
	{{~ if change_source_column != null ~}}

	SET @ChangeSource = CAST(SESSION_CONTEXT(N'{{change_context_change_source_name}}') AS {{string.upcase change_source_column.type}})
	IF @ChangeSource IS NULL
	BEGIN
		;THROW 130101, '{{change_context_change_source_name}} must be set in session context for change tracking. Transaction was not commited.', 1
	END
	{{~ end ~}}
	{{~ if changed_by_column != null ~}}

	SET @ChangedBy = CAST(SESSION_CONTEXT(N'{{change_context_changed_by_name}}') AS {{string.upcase changed_by_column.type}})
	IF @ChangedBy IS NULL
	BEGIN
		;THROW 130101, '{{change_context_changed_by_name}} must be set in session context for change tracking. Transaction was not commited.', 1
	END
	{{~ end ~}}

	{{~ for change_config in change_configs ~}}
	IF @OperationTypeId = {{change_config.operation_type_id}}
	BEGIN
		INSERT INTO dbo.[{{change_table_name}}] ([{{operation_type_column.name}}],
{{- if change_source_column != null -}}
[{{change_source_column.name}}],
{{- end -}}
[{{changed_at_column.name}}],
{{- if changed_by_column != null -}}
[{{changed_by_column.name}}],
{{- end -}}
		{{- array.each change_table_data_columns @format_sql_identifier | array.join "," }})
		{{~
		if change_config.table_alias == "i"
			format_column = @format_inserted_column
		else if change_config.table_alias == "d"
			format_column = @format_deleted_column
		end
		~}}
		SELECT @OperationTypeId,
{{- if change_source_column != null -}}
@ChangeSource,
{{- end -}}
GETUTCDATE(),
{{- if changed_by_column != null -}}
@ChangedBy,
{{- end -}}
		{{- array.each change_table_data_columns @format_column | array.join "," }}
		FROM {{change_config.table_name}} [{{change_config.table_alias}}]
		{{~ if change_config.operation_type_id == 2 ~}}
		JOIN deleted [d] ON {{ array.each primary_key_column_names @build_join | array.join " AND " }}
		WHERE EXISTS (SELECT [i].* EXCEPT SELECT [d].*) -- Only select rows that have changed values
		{{~ end ~}}
	END

	{{~ end ~}}
END;