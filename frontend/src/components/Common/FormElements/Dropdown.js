import { TextField, Autocomplete } from "@mui/material";

function Dropdown({ label, menuItems, value, onChange, sx, onBlur, error, disabled, helperText }) {
    const id = `${label}-select`;
    const options = menuItems.map(x => ({ id: x.id, label: x.value || x.displayName }));

    return (
        <Autocomplete
            id={id}
            options={options}
            value={value}
            sx={sx}
            disabled={disabled}
            size="small"
            isOptionEqualToValue={(o, v) => o.id === v.id}
            onChange={(_, option) => onChange(option)}
            renderInput={(params) =>
                <TextField
                    {...params}
                    label={label}
                    onBlur={onBlur}
                    error={error}
                    helperText={helperText}
                />}
        />
    );
}

export default Dropdown;