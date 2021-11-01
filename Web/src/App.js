import Header from './components/shared/header';
import Main from './components/main';
import React from "react";
import { Redirect } from 'react-router-dom';
import './App.css'

class App extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      token: "",
      loggedIn: false,
      user: {
        username: "",
        email: "",
        id: 0
      },
      redirect: false
    };
  }

  handleLogin(user) {
    this.setState({
      token: user.Token,
      loggedIn: true,
      user: {
        username: user.username,
        email: user.email,
        id: user.userId,
        token: user.token
      }
    });

    this.setState({ redirect: true });
  }

  handleLogout() {
    this.setState({
      token: "",
      loggedIn: false,
      user: {
        username: "",
        email: "",
        id: 0
      }
    });
  }

  render() {
    return (
      <div className="app">
        <Header handleLogout={this.handleLogout.bind(this)}>
          <p>Wassup</p>
        </Header>
        <Main 
          user={this.state.user} 
          handleLogin={this.handleLogin.bind(this)}
        />
        { this.state.redirect ? <Redirect to={{
          pathname: '/',
          user: this.state.user
        }}/> : null }
      </div>
      );
  }
}

export default App;
