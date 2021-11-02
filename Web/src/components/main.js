import React from 'react';
import { Switch, Route } from 'react-router-dom';

import HomePage from './home/homePage';
import LoginPage from './user/loginPage';

const Main = (props) => {
  return (
    <Switch> {/* The Switch decides which component to show based on the current URL.*/}
      <Route exact path='/' component={HomePage} user={props.user}></Route>
      <Route 
        exact path='/login' 
        render={() => <LoginPage 
          user={props.user} 
          handleLogin={props.handleLogin}
        />} 
      />
    </Switch>
  );
}

export default Main;