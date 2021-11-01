import '../shared.css';

function SuccessMessage(props) {
  return(
    <span className="success-message">{props.children}</span>
  )
}

export default SuccessMessage;