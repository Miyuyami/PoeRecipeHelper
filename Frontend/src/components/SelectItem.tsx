import React from "react";
import TextField from "@mui/material/TextField";
import MenuItem from "@mui/material/MenuItem";

interface SelectItemProps {
  label: string;
  value: string;
  options: string[];
  required?: boolean;
  // eslint-disable-next-line no-unused-vars
  onChange: (value: string) => void;
}

// TODO: options to include label
const SelectItem: React.FC<SelectItemProps> = ({ label, value, options, onChange, required }) => {
  return (
    <TextField size="small" select label={label} onChange={(e) => onChange(e.target.value)} required={required} value={value}>
      {options.map((o) => (
        <MenuItem key={o} value={o}>
          {o}
        </MenuItem>
      ))}
    </TextField>
  );
};

export default SelectItem;
