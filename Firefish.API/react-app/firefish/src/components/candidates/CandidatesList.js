import React, { useState, useEffect } from "react";
import PropTypes from "prop-types";
import axios from "axios";
import Card from "@mui/material/Card";
import CardContent from "@mui/material/CardContent";
import Typography from "@mui/material/Typography";
import { CardActions, Button, ButtonGroup, Chip, Divider } from "@mui/material";
import Box from "@mui/material/Box";
import Fab from "@mui/material/Fab";
import AddIcon from "@mui/icons-material/Add";

const useCandidates = () => {
  const [candidates, setCandidates] = useState([]);
  const [skills, setSkills] = useState({});
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchCandidates = async () => {
      try {
        const response = await axios.get(
          "http://localhost:5191/api/candidates",
        );
        setCandidates(response.data);

        // Fetch skills for each candidate
        const skillsPromises = response.data.map((candidate) =>
          axios.get(
            `http://localhost:5191/api/skills/candidate/${candidate.id}`,
          ),
        );

        const skillsResponses = await Promise.all(skillsPromises);
        let skillsData = {};
        skillsResponses.forEach((response, index) => {
          skillsData[index] = response.data;
        });

        setSkills(skillsData);
        setLoading(false);
      } catch (err) {
        console.error("Error fetching data: ", err);
        setError("Failed to fetch data. Please try again later.");
        setLoading(false);
      }
    };

    fetchCandidates();
  }, []);

  return { candidates, skills, loading, error };
};

const Candidate = ({ candidate, skills, index }) => (
  <Card sx={{ minWidth: 275, mb: 2 }}>
    <CardContent>
      <Typography variant="h3" color="primary" component="div">
        <Divider
          sx={{
            "&::before, &::after": {
              border: "1px solid teal"
            },
          }}
        >
          {candidate.name}
        </Divider>
      </Typography>
      <Typography sx={{ mb: 1.5 }} color="text.secondary">
        Born: {new Date(candidate.dateOfBirth).toLocaleDateString()}
      </Typography>
      <Typography variant="body2">
        Town: {candidate.town}
        <br />
        Phone: {candidate.phone}
      </Typography>
      <Typography variant="body2" sx={{ mt: 2 }}>
        Skills:
      </Typography>
      {skills[index] &&
        skills[index].map((skill) => (
          <Chip
            variant="outlined"
            color="primary"
            key={skill.id}
            label={skill.name}
            sx={{ m: 0.5 }}
          />
        ))}
    </CardContent>
    <Divider sx={{ bgcolor: "teal" }} />
    <CardActions sx={{
        display: 'flex',
        flexDirection: 'column',
        alignItems: 'center',
        '& > *': {
          m: 1,
        },
      }}>
      <ButtonGroup variant="outlined">
        <Button>More Details</Button>
        <Button>Edit Candidate</Button>
      </ButtonGroup>
    </CardActions>
  </Card>
);

Candidate.propTypes = {
  candidate: PropTypes.shape({
    id: PropTypes.number,
    name: PropTypes.string,
    dateOfBirth: PropTypes.string,
    town: PropTypes.string,
    phone: PropTypes.string,
  }),
  skills: PropTypes.object,
  index: PropTypes.number,
};

function CandidateList() {
  const { candidates, skills, loading, error } = useCandidates();

  if (loading) return <div>Loading...</div>;
  if (error) return <div>{error}</div>;

  return (
    <div style={{ maxWidth: 900, margin: "16px auto", position: "relative" }}>
      <Typography
        color="primary"
        textAlign="center"
        variant="h2"
        component="h2"
        gutterBottom
      >
        Candidate List
      </Typography>
      {candidates.map((candidate, index) => (
        <Candidate
          key={candidate.id}
          candidate={candidate}
          skills={skills}
          index={index}
        />
      ))}
      <Box sx={{ position: "fixed", bottom: 16, right: 16 }}>
        <Fab variant="extended" size="large" color="secondary" aria-label="add">
          New Candidate
          <AddIcon />
        </Fab>
      </Box>
    </div>
  );
}

export default CandidateList;
