
import React from 'react';
import { BrowserRouter as Router, Route, Switch } from 'react-router-dom';
import CandidatesList from './components/candidates/CandidatesList';
import AddCandidate from './components/candidates/AddCandidate';
import EditCandidate from './components/candidates/EditCandidate';
// import SkillsList from './components/SkillsList';
// import AddSkill from './components/AddSkill';
// import RemoveSkill from './components/RemoveSkill';

function App() {
  return (
    <Router>
      <div className="App">
        <Switch>
          <Route exact path="/" component={CandidatesList} />
          <Route path="/add" component={AddCandidate} />
          <Route path="/edit/:id" component={EditCandidate} />
        </Switch>
      </div>
    </Router>
  );
}

export default App;