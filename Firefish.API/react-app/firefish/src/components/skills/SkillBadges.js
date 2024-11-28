import React from 'react';
import PropTypes from 'prop-types';
import { Chip } from '@mui/material';
import axios from 'axios';

// Reusable skills component to render deletable skill badges for a candidate 

const SkillBadges = ({ skills, onSkillRemoved }) => {
  const removeSkillFromCandidate = async (candidateSkillId) => {
    try {
      await axios.delete(`http://localhost:5191/api/skills/${candidateSkillId}`);
      // Call the onSkillRemoved callback to update the parent component's state
      onSkillRemoved(candidateSkillId);
    } catch (error) {
      console.error('Error removing skill:', error);
      // You might want to add some error handling here, such as showing an error message to the user
    }
  };
  return (
    <>
      {skills && skills.map((skill) => (
        <Chip
          variant="outlined"
          color="primary"
          key={skill.candidateSkillId}
          label={skill.name}
          sx={{ m: 0.5 }}
          onDelete={() => removeSkillFromCandidate(skill.candidateSkillId)}
        />
      ))}
    </>
  );
};

SkillBadges.propTypes = {
  skills: PropTypes.arrayOf(
    PropTypes.shape({
      candidateSkillId: PropTypes.number.isRequired,
      skillId: PropTypes.number.isRequired,
      name: PropTypes.string.isRequired,
    })
  ).isRequired,
  onSkillRemoved: PropTypes.func.isRequired,
};

export default SkillBadges;
