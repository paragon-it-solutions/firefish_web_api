import React, { useState, useEffect } from 'react';
import PropTypes from 'prop-types';
import axios from 'axios';
import styled from 'styled-components';

const ListContainer = styled.div`
  max-width: 800px;
  margin: 0 auto;
`;

const List = styled.ul`
  list-style-type: none;
  padding: 0;
`;

const ListItem = styled.li`
  background-color: #f0f0f0;
  margin-bottom: 10px;
  padding: 10px;
  border-radius: 5px;
`;

const useCandidates = () => {
  const [candidates, setCandidates] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchCandidates = async () => {
      try {
        const response = await axios.get('http://localhost:5191/api/candidates');
        setCandidates(response.data);
        setLoading(false);
      } catch (err) {
        console.error('Error fetching candidates: ', err);
        setError('Failed to fetch candidates. Please try again later.');
        setLoading(false);
      }
    };

    fetchCandidates();
  }, []);

  return { candidates, loading, error };
};

const Candidate = ({ candidate }) => (
  <ListItem>
    <strong>{candidate.name}</strong> - Born: {new Date(candidate.dateOfBirth).toLocaleDateString()} - 
    Town: {candidate.town} - Phone: {candidate.phone}
  </ListItem>
);

Candidate.propTypes = {
  candidate: PropTypes.shape({
    id: PropTypes.number.isRequired,
    name: PropTypes.string.isRequired,
    dateOfBirth: PropTypes.string.isRequired,
    town: PropTypes.string,
    phone: PropTypes.string
  }).isRequired
};

function CandidateList() {
  const { candidates, loading, error } = useCandidates();

  if (loading) return <div>Loading...</div>;
  if (error) return <div>{error}</div>;

  return (
    <ListContainer>
      <h2>Candidate List</h2>
      <List>
        {candidates.map(candidate => (
          <Candidate key={candidate.id} candidate={candidate} />
        ))}
      </List>
    </ListContainer>
  );
}

export default CandidateList;