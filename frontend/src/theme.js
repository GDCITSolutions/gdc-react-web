import { createTheme } from "@mui/material";

const font = 'Open Sans, Roboto, -apple-system, BlinkMacSystemFont,"Helvetica Neue", "Segoe UI", Arial, sans-serif';

export default createTheme({
    palette: {
        mode: 'light',
        primary: {
            main: '#011E41'
        },
        secondary: {
            main: '#0180ff'
        },
        tertiary: {
            main: '#4d4d4d'
        },
        danger: {
            main: "#BA0B0B",
            light: "#ff0000",
            contrastText: "#FFFFFF"
        }
    },
    typography: {
        fontFamily: font,
        button: {
            textTransform: 'none'
        }
    },
    components: {
        MuiCheckbox: {
            root: {
                colorPrimary: {
                    color: '#007483',
                }
            }
        },
        MuiLink: {
            styleOverrides: {
                root: {
                    fontFamily: font
                }
            }
        },
        MuiTabs: {
            styleOverrides: {
              indicator: {
                backgroundColor: '#007483',
              }
            }
        },
        MuiTextField: {
            styleOverrides: {
                root: {
                    '& .MuiOutlinedInput-root': {
                        '& fieldset': {
                          borderColor: '#909090',
                        }
                    },
                }
            }
        }
    },
})