import React from "react";
import ServerError from "../shared/messages/serverError";
import ValidationError from "../shared/messages/validationError";
import SuccessMessage from "../shared/messages/successMessage";
import axios from "axios";
import './home.css'

class EntryFrom extends React.Component {
  constructor(props) {
		super(props);
		this.state = {
      fields: {
        "name": "", 
        "phone":""
      },
      errors: {},
			duplicateMessage: "",
      successMessage: ""
		};
	}

  handleChange(field, e) {
    let fields = this.state.fields;
    fields[field] = e.target.value;
    this.setState({ fields });
		this.setState({
      duplicateMessage: "",
      successMessage: ""
    })
  }

  handleValidation() {
		let formValid = true;
    let errors = {};
    let fields = this.state.fields;

		if (!fields["name"]) {
      formValid = false;
      errors["name"] = "Enter a name";
    } else if (!/^[a-zA-Z\s]*$/.test(fields["name"])) {
      formValid = false;
      errors["name"] = "Name must consist of letters and whitespace only";
    }

		if (!fields["phone"]) {
      formValid = false;
      errors["phone"] = "Enter your phone";
    } else if (!/^[\d+]+$/.test(fields["phone"])) {
      formValid = false;
      errors["phone"] = "Phone must consist of numbers and + sign only";
    } else if (fields["phone"].length < 9) {
      formValid = false;
      errors["phone"] = "Phone must be longer than 9 symbols";
    } 

		this.setState({ errors: errors });
    return formValid;
	}

  collectData(e) {
		e.preventDefault();

		if (!this.handleValidation())
      return;

    const config = {
      headers: { Authorization: `Bearer ${this.props.user.token}` }
    }

    const _this = this;
    axios.post('https://localhost:5001/api/Entry/', this.state.fields, config)
    .then(function (response) {
      console.log(response);
      _this.setState({ 
        successMessage: "Entry has been added successfully.",
        fields: {
          name: "",
          phone: ""
        }
      })
    })
    .catch(function (error) {
      _this.setState({ duplicateMessage: "Number is already in contact list."});
      console.log(error);
    });
	}

  render() {
    return(
      <div className="entry-form">
        <h2>New Entry</h2>

        <label>Name</label>
				<input 
					type="text"
					value={this.state.fields["name"]}
					onChange={this.handleChange.bind(this, "name")}
					maxLength="30"
				/>
				<ValidationError>{this.state.errors["name"]}</ValidationError>

				<label>Phone</label>
				<input 
					type="text"
					value={this.state.fields["phone"]}
					onChange={this.handleChange.bind(this, "phone")}
					maxLength="12"
				/>
				<ValidationError>{this.state.errors["phone"]}</ValidationError>

				<input type="submit" value="CREATE" onClick={ this.collectData.bind(this) }/>
				<ServerError>{this.state.duplicateMessage}</ServerError>
        <SuccessMessage>{this.state.successMessage}</SuccessMessage>
      </div>
    )
  }
}

export default EntryFrom;