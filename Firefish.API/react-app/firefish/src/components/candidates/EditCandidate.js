import React, { useState, useEffect } from "react";
import {
  TextField,
  Button,
  ButtonGroup,
  Box,
  Typography,
  CircularProgress,
} from "@mui/material";
import axios from "axios";
import { useParams, useNavigate } from "react-router-dom";

import apiBase from '../shared/ApiConfig';

const EditCandidateForm = () => {
  const { candidateId } = useParams();
  const navigate = useNavigate();
  const [formData, setFormData] = useState({
    firstName: "",
    surname: "",
    dateOfBirth: "",
    address: "",
    town: "",
    country: "",
    postCode: "",
    phoneHome: "",
    phoneMobile: "",
    phoneWork: "",
  });

  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");
  const [success, setSuccess] = useState("");

  useEffect(() => {
    const fetchCandidateData = async () => {
      try {
        const response = await axios.get(
          `${apiBase}/candidates/${candidateId}`,
        );
        const candidateData = response.data;
        const [firstName, ...surnameArray] = candidateData.name.split(" ");
        const surname = surnameArray.join(" ");
        setFormData({
          ...candidateData,
          firstName,
          surname,
          dateOfBirth: new Date(candidateData.dateOfBirth)
            .toISOString()
            .split("T")[0],
        });
        setLoading(false);
      } catch (err) {
        console.error("Error fetching candidate data:", err);
        setError("Failed to fetch candidate data. Please try again.");
        setLoading(false);
      }
    };

    fetchCandidateData();
  }, [candidateId]);

  const handleChange = (e) => {
    setFormData({ ...formData, [e.target.name]: e.target.value });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError("");
    setSuccess("");

    try {
      await axios.put(`${apiBase}/candidates/${candidateId}`, {
        ...formData,
        name: `${formData.firstName} ${formData.surname}`,
        dateOfBirth: new Date(formData.dateOfBirth).toISOString(),
      });
      setSuccess("Candidate updated successfully!");
      setTimeout(() => navigate("/"), 2000); // Redirect to home page after 2 seconds
    } catch (err) {
      setError("Failed to update candidate. Please try again.");
      console.error("Error updating candidate:", err);
    }
  };

  if (loading) {
    return <CircularProgress />;
  }

  return (
    <Box
      component="form"
      onSubmit={handleSubmit}
      sx={{ maxWidth: 400, margin: "auto", mt: 4 }}
    >
      <Typography variant="h4" component="h2" gutterBottom>
        Edit Candidate
      </Typography>
      <TextField
        fullWidth
        label="First Name"
        name="firstName"
        value={formData.firstName}
        onChange={handleChange}
        margin="normal"
        required
      />
      <TextField
        fullWidth
        label="Surname"
        name="surname"
        value={formData.surname}
        onChange={handleChange}
        margin="normal"
        required
      />
      <TextField
        fullWidth
        label="Date of Birth"
        name="dateOfBirth"
        type="date"
        value={formData.dateOfBirth}
        onChange={handleChange}
        margin="normal"
        required
        InputLabelProps={{ shrink: true }}
      />
      <TextField
        fullWidth
        label="Address"
        name="address"
        value={formData.address}
        onChange={handleChange}
        margin="normal"
      />
      <TextField
        fullWidth
        label="Town"
        name="town"
        value={formData.town}
        onChange={handleChange}
        margin="normal"
      />
      <TextField
        fullWidth
        label="Country"
        name="country"
        value={formData.country}
        onChange={handleChange}
        margin="normal"
      />
      <TextField
        fullWidth
        label="Post Code"
        name="postCode"
        value={formData.postCode}
        onChange={handleChange}
        margin="normal"
      />
      <TextField
        fullWidth
        label="Home Phone"
        name="phoneHome"
        value={formData.phoneHome}
        onChange={handleChange}
        margin="normal"
      />
      <TextField
        fullWidth
        label="Mobile Phone"
        name="phoneMobile"
        value={formData.phoneMobile}
        onChange={handleChange}
        margin="normal"
      />
      <TextField
        fullWidth
        label="Work Phone"
        name="phoneWork"
        value={formData.phoneWork}
        onChange={handleChange}
        margin="normal"
      />
      <ButtonGroup>
        <Button href="/" variant="contained" color="secondary">
          Cancel
        </Button>
        <Button type="submit" variant="contained" color="primary">
          Update Candidate
        </Button>
      </ButtonGroup>
      {error && (
        <Typography color="error" sx={{ mt: 2 }}>
          {error}
        </Typography>
      )}
      {success && (
        <Typography color="success" sx={{ mt: 2 }}>
          {success}
        </Typography>
      )}
    </Box>
  );
};

export default EditCandidateForm;
