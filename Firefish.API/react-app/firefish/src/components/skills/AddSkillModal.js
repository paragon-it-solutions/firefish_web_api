import React, { useState, useEffect } from "react";
import PropTypes from "prop-types";
import {
  Typography,
  Alert,
  InputLabel,
  Modal,
  CircularProgress,
  Box,
  FormControl,
  Select,
  Snackbar,
  Button,
  MenuItem,
} from "@mui/material";
import axios from "axios";

const style = {
  position: "absolute",
  top: "50%",
  left: "50%",
  transform: "translate(-50%, -50%)",
  width: 400,
  bgcolor: "background.paper",
  border: "2px solid #000",
  boxShadow: 24,
  p: 4,
};

export default function AddSkillModal({ candidateId, candidateName }) {
  const [open, setOpen] = useState(false);
  const [skills, setSkills] = useState([]);
  const [selectedSkill, setSelectedSkill] = useState("");
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);
  const [snackbar, setSnackbar] = useState({
    open: false,
    message: "",
    severity: "success",
  });

  const handleOpen = () => setOpen(true);
  const handleClose = () => {
    setOpen(false);
    setSelectedSkill("");
    setError(null);
  };

  useEffect(() => {
    if (open) {
      fetchSkills();
    }
  }, [open]);

  const fetchSkills = async () => {
    setLoading(true);
    setError(null);
    try {
      const response = await axios.get("http://localhost:5191/api/skills");
      setSkills(response.data);
    } catch (err) {
      console.error("Error fetching skills:", err);
      setError("Failed to fetch skills. Please try again.");
    } finally {
      setLoading(false);
    }
  };

  const handleSkillChange = (event, newValue) => {
    console.log("Event:", event);
    // id of skill returned with . infront, strip out
    const key = newValue.key.startsWith(".")
      ? newValue.key.substring(1)
      : newValue.key;
    setSelectedSkill(parseInt(key));
  };

  const handleAddSkill = async () => {
    setLoading(true);
    setError(null);
    try {
      await axios.post("http://localhost:5191/api/skills", {
        candidateId: candidateId,
        skillId: selectedSkill,
      });
      setSnackbar({
        open: true,
        message: "Skill added successfully!",
        severity: "success",
      });
      handleClose();
    } catch (err) {
      console.error("Error adding skill:", err);
      setError("Failed to add skill. Please try again.");
      setSnackbar({
        open: true,
        message: "Failed to add skill. Please try again.",
        severity: "error",
      });
    } finally {
      setLoading(false);
    }
  };

  const handleSnackbarClose = (event, reason) => {
    if (reason === "clickaway") {
      return;
    }
    setSnackbar({ ...snackbar, open: false });
  };

  return (
    <>
      <Button onClick={handleOpen}>Add Skill</Button>
      <Modal open={open} onClose={handleClose}>
        <Box sx={style}>
          <Typography variant="h6" component="h2">
            Add Skill for {candidateName}
          </Typography>
          <Typography sx={{ mt: 2, mb: 2 }}>
            Candidate ID: {candidateId}
          </Typography>
          {loading ? (
            <CircularProgress />
          ) : error ? (
            <Typography color="error">{error}</Typography>
          ) : (
            <FormControl fullWidth>
              <InputLabel id="skill-select-label">Skill</InputLabel>
              <Select
                labelId="skill-select-label"
                id="skill-select"
                value={selectedSkill}
                label="Skill"
                onChange={handleSkillChange}
              >
                {skills.map((skill) => (
                  <MenuItem key={skill.id} value={skill.id}>
                    {skill.name}
                  </MenuItem>
                ))}
              </Select>
            </FormControl>
          )}
          <Button
            sx={{ mt: 2 }}
            variant="contained"
            color="success"
            onClick={handleAddSkill}
            disabled={!selectedSkill || loading}
          >
            {loading ? "Adding..." : "Confirm"}
          </Button>
        </Box>
      </Modal>
      <Snackbar
        open={snackbar.open}
        autoHideDuration={6000}
        onClose={handleSnackbarClose}
      >
        <Alert
          onClose={handleSnackbarClose}
          severity={snackbar.severity}
          sx={{ width: "100%" }}
        >
          {snackbar.message}
        </Alert>
      </Snackbar>
    </>
  );
}

AddSkillModal.propTypes = {
  candidateId: PropTypes.number.isRequired,
  candidateName: PropTypes.string.isRequired,
};
