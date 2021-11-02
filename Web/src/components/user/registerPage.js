import React from "react";
import GenericPage from "../shared/genericPage";
import SuccessMessage from "../shared/messages/successMessage";
import ValidationError from "../shared/messages/validationError";
import ServerError from "../shared/messages/serverError";
import axios from "axios";
import { Link } from "react-router-dom";

class RegisterPage extends React.Component {
  constructor(props) {
		super(props);
		this.state = {
      fields: {
        "email": "", 
        "password":"",
        "username": ""
      },
      errors: {},
			serverErrorMessage: "",
      successMessage: ""
		};
	}

  handleChange(field, e) {
    let fields = this.state.fields;
    fields[field] = e.target.value;
    this.setState({ fields });
		this.setState({
      serverErrorMessage: "",
      successMessage: ""
    })
  }

  handleValidation() {
		let formValid = true;
    let errors = {};
    let fields = this.state.fields;

    if (!fields["username"]) {
      formValid = false;
      errors["username"] = "Enter an username";
    } else if (fields["username"].length < 3) {
      formValid = false;
      errors["username"] = "User name must contain 3 or more symbols";
    } else if (!fields["username"].match("^[A-Za-z0-9]+$")) {
      formValid = false;
      errors["username"] = "User name must consist of letters and digits only";
    }

		if (!fields["email"]) {
      formValid = false;
      errors["email"] = "Enter your email address";
    } else if (fields["email"].length < 3 || 
							!fields["email"].includes('@') ||
							!fields["email"].includes('.')) {
      formValid = false;
      errors["email"] = "Enter a valid email addess";
    }

		if (!fields["password"]) {
      formValid = false;
      errors["password"] = "Enter a password";
    } else if (fields["password"].length < 8) {
      formValid = false;
      errors["password"] = "Password must contain 8 or more symbols";
    }

		this.setState({ errors: errors });
    return formValid;
	}

  collectData(e) {
		e.preventDefault();

		if (!this.handleValidation())
      return;

      const _this = this;
      axios.post('https://localhost:5001/api/User/Create', this.state.fields)
      .then(function (response) {
        _this.setState({ successMessage: "Your profile has been created. Please log in."});
      })
      .catch(function (error) {
        _this.setState({ serverErrorMessage: "This username or email is unavailable"});
        console.log(error);
      });
	}

  render() {
    return(
      <GenericPage>
				<h2>Create an account</h2>

				<label>User name</label>
				<input 
					type="text"
					value={this.state.fields["username"]}
					onChange={this.handleChange.bind(this, "username")}
					maxLength="20"
				/>
				<ValidationError>{this.state.errors["username"]}</ValidationError>

				<label>Email address</label>
				<input 
					type="text"
					value={this.state.fields["email"]}
					onChange={this.handleChange.bind(this, "email")}
					maxLength="30"
				/>
				<ValidationError>{this.state.errors["email"]}</ValidationError>

				<label>Password</label>
				<input 
					type="password"
					value={this.state.fields["password"]}
					onChange={this.handleChange.bind(this, "password")}
					maxLength="30"
				/>
				<ValidationError>{this.state.errors["password"]}</ValidationError>

				<input type="submit" value="CREATE" onClick={ this.collectData.bind(this) }/>
				<ServerError>{this.state.serverErrorMessage}</ServerError>
        <SuccessMessage>{this.state.successMessage}</SuccessMessage>

				<Link to="/login">
					<div className="link">Log in</div>
				</Link>
			</GenericPage>
    )
  }
}

export default RegisterPage;