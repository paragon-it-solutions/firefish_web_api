import React, { useState } from "react";
import PropTypes from "prop-types";
import { Chip } from "@mui/material";
import axios from "axios";
import red from "@mui/material/colors/red";

// Custom Components
import AlertDanger from "../shared/AlertDanger";
import apiBase from "../shared/ApiConfig";

const SkillBadges = ({ skills, onSkillRemoved }) => {
  const [alertMessage, setAlertMessage] = useState("");
  const [isAlertOpen, setIsAlertOpen] = useState(false);

  const removeSkillFromCandidate = async (candidateSkillId) => {
    try {
      await axios.delete(
        `${apiBase}/skills/${candidateSkillId}`,
      );
      // Call the onSkillRemoved callback to update the parent component's state
      onSkillRemoved(candidateSkillId);
    } catch (error) {
      console.error("Error removing skill:", error);
      setAlertMessage("Failed to remove skill. Please try again.");
      setIsAlertOpen(true);
    }
  };

  const handleCloseAlert = () => {
    setIsAlertOpen(false);
    setAlertMessage("");
  };
  return (
    <>
      {skills &&
        skills.map((skill) => (
          <Chip
            key={skill.candidateSkillId}
            label={skill.name}
            onDelete={() => removeSkillFromCandidate(skill.candidateSkillId)}
            sx={{
              margin: "2px",
              "&:hover": {
                backgroundColor: red[500],
                color: "white", // Blue text on hover
              },
            }}
          />
        ))}
      <AlertDanger
        message={alertMessage}
        open={isAlertOpen}
        onClose={handleCloseAlert}
      />
    </>
  );
};

SkillBadges.propTypes = {
  skills: PropTypes.arrayOf(
    PropTypes.shape({
      id: PropTypes.number.isRequired,
      name: PropTypes.string.isRequired,
    }),
  ).isRequired,
  candidateId: PropTypes.number.isRequired,
  onSkillRemoved: PropTypes.func,
};

export default SkillBadges;
