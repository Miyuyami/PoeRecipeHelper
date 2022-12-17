import React from "react";
import TextField from "@mui/material/TextField";

interface InputProps {
  label: string;
  value: string;
  required?: boolean;
  // eslint-disable-next-line no-unused-vars
  onChange: (value: string) => void;
}

// TODO: debounce
const TextInput: React.FC<InputProps> = ({ label, value, onChange, required }) => {
  return <TextField size="small" label={label} value={value} onChange={(e) => onChange(e.target.value)} required={required} />;
};

export default TextInput;
