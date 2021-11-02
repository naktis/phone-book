import React from 'react';
import { Switch, Route } from 'react-router-dom';

import HomePage from './home/homePage';
import LoginPage from './user/loginPage';
import RegisterPage from './user/registerPage';

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
      <Route exact path='/register' component={RegisterPage}></Route>
    </Switch>
  );
}

export default Main;