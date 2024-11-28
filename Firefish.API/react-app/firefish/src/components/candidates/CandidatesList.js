import React, { useState, useEffect } from "react";
import PropTypes from "prop-types";
import axios from "axios";
import Card from "@mui/material/Card";
import CardContent from "@mui/material/CardContent";
import Typography from "@mui/material/Typography";
import { CardActions, Button, ButtonGroup, Divider } from "@mui/material";
import Box from "@mui/material/Box";
import Fab from "@mui/material/Fab";
import AddIcon from "@mui/icons-material/Add";

// Custom Components
import SkillBadges from "../skills/SkillBadges";
import AddSkillModal from "../skills/AddSkillModal";
import AlertDanger from "../shared/AlertDanger";
import apiBase from '../shared/ApiConfig';

const useCandidates = () => {
  const [candidates, setCandidates] = useState([]);
  const [skills, setSkills] = useState({});
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchCandidates = async () => {
      try {
        const response = await axios.get(
          `${apiBase}/candidates`,
        );
        setCandidates(response.data);

        // Fetch skills for each candidate
        const skillsPromises = response.data.map((candidate) =>
          axios.get(
            `${apiBase}/skills/candidate/${candidate.id}`,
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

  const fetchCandidateDetails = async (candidateId) => {
    try {
      const response = await axios.get(
        `${apiBase}/candidates/${candidateId}`,
      );
      return response.data;
    } catch (err) {
      console.error("Error fetching candidate details: ", err);
      throw err;
    }
  };

  return {
    candidates,
    skills,
    setSkills,
    loading,
    error,
    fetchCandidateDetails,
  };
};

const Candidate = ({
  candidate,
  candidateDetails,
  skills,
  onMoreDetails,
  onSkillRemoved,
}) => {
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState(null);

  const handleMoreDetails = async () => {
    if (candidateDetails) return; // Don't fetch if we already have details
    setIsLoading(true);
    setError(null);
    try {
      await onMoreDetails(candidate.id);
    } catch (err) {
      setError("Failed to fetch more details. Please try again.");
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <Card sx={{ minWidth: 275, mb: 2 }}>
      <CardContent>
        <Typography variant="h3" color="primary" component="div">
          <Divider
            sx={{
              "&::before, &::after": {
                border: "1px solid teal",
              },
            }}
          >
            {candidate.name}
          </Divider>
        </Typography>
        {!candidateDetails ? (
          // Basic candidate information
          <Box
            sx={{
              display: "flex",
              justifyContent: "space-between",
              mt: 2,
            }}
          >
            <Typography variant="h5">
              Born: {new Date(candidate.dateOfBirth).toLocaleDateString()}
            </Typography>
            <Typography variant="h5">Town: {candidate.town}</Typography>
            <Typography variant="h5">Phone: {candidate.phone}</Typography>
          </Box>
        ) : (
          // Detailed candidate information
          <Box
            sx={{
              display: "flex",
              justifyContent: "space-between",
              mt: 2,
            }}
          >
            <Typography variant="h5">
              Born:{" "}
              {new Date(candidateDetails.dateOfBirth).toLocaleDateString()}
            </Typography>
            <Typography variant="h5">
              Address: {candidateDetails.address}
            </Typography>
            <Typography variant="h5">Town: {candidateDetails.town}</Typography>
            <Typography variant="h5">
              Country: {candidateDetails.country}
            </Typography>
            <Typography variant="h5">
              Post Code: {candidateDetails.postCode}
            </Typography>
            <Typography variant="h5">
              Phone (Home): {candidateDetails.phoneHome}
            </Typography>
            <Typography variant="h5">
              Phone (Mobile): {candidateDetails.phoneMobile}
            </Typography>
            <Typography variant="h5">
              Phone (Work): {candidateDetails.phoneWork}
            </Typography>
          </Box>
        )}
        <Typography variant="body2" sx={{ mt: 2, mb: 1 }}>
          Skills:
        </Typography>
        <SkillBadges
          skills={skills}
          candidateId={candidate.id}
          candidateName={candidate.name}
          onSkillRemoved={(skillId) => onSkillRemoved(candidate.id, skillId)}
        />
      </CardContent>
      <Divider sx={{ bgcolor: "teal" }} />
      <CardActions
        sx={{
          display: "flex",
          flexDirection: "column",
          alignItems: "center",
          "& > *": {
            m: 1,
          },
        }}
      >
        <ButtonGroup variant="outlined">
          <Button
            onClick={handleMoreDetails}
            disabled={isLoading || !!candidateDetails}
          >
            {isLoading
              ? "Loading..."
              : candidateDetails
                ? "Details Loaded"
                : "More Details"}
          </Button>
          <Button href={`/edit/${candidate.id}`}>Edit Candidate</Button>
          <AddSkillModal
            candidateId={candidate.id}
            candidateName={candidate.name}
          />
        </ButtonGroup>
        {error && (
          <Typography color="error" variant="body2">
            {error}
          </Typography>
        )}
      </CardActions>
    </Card>
  );
};

Candidate.propTypes = {
  // properties for base candidate object
  candidate: PropTypes.shape({
    id: PropTypes.number.isRequired,
    name: PropTypes.string,
    dateOfBirth: PropTypes.string,
    town: PropTypes.string,
    phone: PropTypes.string,
  }).isRequired,
  // properties for candidate details object
  candidateDetails: PropTypes.shape({
    id: PropTypes.number.isRequired,
    name: PropTypes.string,
    dateOfBirth: PropTypes.string,
    address: PropTypes.string,
    town: PropTypes.string,
    country: PropTypes.string,
    postCode: PropTypes.string,
    phoneHome: PropTypes.string,
    phoneMobile: PropTypes.string,
    phoneWork: PropTypes.string,
    createdDate: PropTypes.string.isRequired,
    updatedDate: PropTypes.string.isRequired,
  }),
  skills: PropTypes.array.isRequired,
  onMoreDetails: PropTypes.func.isRequired,
  onSkillRemoved: PropTypes.func.isRequired,
};

function CandidateList() {
  const {
    candidates,
    skills,
    setSkills,
    loading,
    error,
    fetchCandidateDetails,
  } = useCandidates();
  const [candidateDetails, setCandidateDetails] = useState({});
  const [alertMessage, setAlertMessage] = useState("");
  const [isAlertOpen, setIsAlertOpen] = useState(false);

  const handleMoreDetails = async (candidateId) => {
    try {
      const details = await fetchCandidateDetails(candidateId);
      setCandidateDetails((prevDetails) => ({
        ...prevDetails,
        [candidateId]: details,
      }));
    } catch (err) {
      console.error("Error fetching candidate details: ", err);
      setAlertMessage("Error fetching candidate details");
      setIsAlertOpen(true);
    }
  };

  const handleSkillRemoved = (candidateId, skillId) => {
    setSkills((prevSkills) => ({
      ...prevSkills,
      [candidateId]: prevSkills[candidateId].filter(
        (skill) => skill.candidateSkillId !== skillId,
      ),
    }));
  };

  const handleCloseAlert = () => {
    setIsAlertOpen(false);
    setAlertMessage("");
  };

  useEffect(() => {
    if (error) {
      setAlertMessage(error);
      setIsAlertOpen(true);
    }
  }, [error]);

  if (loading) return <div>Loading...</div>;

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
      {candidates.map((candidate) => (
        <Candidate
          key={candidate.id}
          candidate={candidate}
          candidateDetails={candidateDetails[candidate.id]}
          skills={skills[candidate.id]}
          onMoreDetails={handleMoreDetails}
          onSkillRemoved={handleSkillRemoved}
        />
      ))}
      <Box sx={{ position: "fixed", bottom: 16, right: 16 }}>
        <Fab
          href="/add"
          variant="extended"
          size="large"
          color="secondary"
          aria-label="add"
        >
          New Candidate
          <AddIcon />
        </Fab>
      </Box>
      <AlertDanger
        message={alertMessage}
        open={isAlertOpen}
        onClose={handleCloseAlert}
      />
    </div>
  );
}

export default CandidateList;
