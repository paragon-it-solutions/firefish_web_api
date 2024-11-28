import React from "react";
import { BrowserRouter as Router, Route, Routes } from "react-router-dom";
import { ThemeProvider, createTheme } from "@mui/material/styles";
import { teal } from "@mui/material/colors";
import CssBaseline from "@mui/material/CssBaseline";
import CandidatesList from "./components/candidates/CandidatesList";
import AddCandidate from "./components/candidates/AddCandidate";
import EditCandidate from "./components/candidates/EditCandidate";
// import SkillsList from './components/SkillsList';
// import AddSkill from './components/AddSkill';
// import RemoveSkill from './components/RemoveSkill';

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
            <Route path="/add" element={<AddCandidate />} />
            <Route path="/edit/:id" element={<EditCandidate />} />
          </Routes>
        </div>
      </Router>
    </ThemeProvider>
  );
}

export default App;
