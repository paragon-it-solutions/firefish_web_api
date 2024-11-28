import React from "react";
import { BrowserRouter as Router, Route, Routes } from "react-router-dom";
import { ThemeProvider, createTheme } from "@mui/material/styles";
import { teal } from "@mui/material/colors";
import CssBaseline from "@mui/material/CssBaseline";
import CandidatesList from "./components/candidates/CandidatesList";
import AddCandidateForm from "./components/candidates/AddCandidateForm";
import EditCandidateForm from "./components/candidates/EditCandidate";
// Create a dark theme
const darkTheme = createTheme({
  palette: {
    mode: "dark",
    primary: teal,
    secondary: {
      main: "#ff7043",
    },
  },
});
function App() {
  return (
    <ThemeProvider theme={darkTheme}>
      <CssBaseline />
      <Router>
        <div className="App">
          <Routes>
            <Route path="/" element={<CandidatesList />} />
            <Route path="/add" element={<AddCandidateForm />} />
            <Route path="/edit/:candidateId" element={<EditCandidateForm />} />
          </Routes>
        </div>
      </Router>
    </ThemeProvider>
  );
}

export default App;
