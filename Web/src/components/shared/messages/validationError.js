import '../shared.css';

function ValidationError(props) {
  return(
    <span className="validation-error">{props.children}</span>
  )
}

export default ValidationError;