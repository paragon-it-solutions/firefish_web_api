import React, { useState } from "react";
import { TextField, Button, ButtonGroup, Box, Typography } from "@mui/material";
import axios from "axios";

const AddCandidateForm = () => {
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

  const [error, setError] = useState("");
  const [success, setSuccess] = useState("");

  const handleChange = (e) => {
    setFormData({ ...formData, [e.target.name]: e.target.value });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError("");
    setSuccess("");

    try {
      const response = await axios.post(
        "http://localhost:5191/api/candidates",
        {
          ...formData,
          dateOfBirth: new Date(formData.dateOfBirth).toISOString(),
        },
      );
      setSuccess("Candidate created successfully!");
      setFormData({
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
    } catch (err) {
      setError("Failed to create candidate. Please try again.");
      console.error("Error creating candidate:", err);
    }
  };

  return (
    <Box
      component="form"
      onSubmit={handleSubmit}
      sx={{ maxWidth: 400, margin: "auto", mt: 4 }}
    >
      <Typography variant="h4" component="h2" gutterBottom>
        Add New Candidate
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
        <Button href="/" variant="contained outline" color="danger">
          Cancel
        </Button>
        <Button type="submit" variant="contained" color="primary">
          Add Candidate
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

export default AddCandidateForm;
