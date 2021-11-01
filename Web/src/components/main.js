import React from 'react';
import { Switch, Route } from 'react-router-dom';

import HomePage from './home/homePage';
import LoginPage from './publicPages/loginPage';

const Main = (props) => {
  return (
    <Switch> {/* The Switch decides which component to show based on the current URL.*/}
      <Route exact path='/' component={HomePage} user={props.user}></Route>
      <Route 
        exact path='/login' 
        render={() => <LoginPage user={props.user} handleLogin={props.handleLogin}/>} 
      />
    </Switch>
  );
}

export default Main;

/*      <Route exact path="/create" component={CreatePage} />
      <Route exact path="/view/:id" component={ViewPage} />
      <Route exact path="/edit" component={EditPage} />
      <Route exact path="/categories" component={EditCategoryPage} />
      <Route exact path="/companies" component={EditCompanyPage} />
              component={LoginPage} 
        user={props.user}
        handleLogin={props.handleLogin}*/